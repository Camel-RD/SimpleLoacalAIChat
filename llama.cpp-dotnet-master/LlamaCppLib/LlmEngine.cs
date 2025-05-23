using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using static LlamaCppLib.Native;
using static LlamaCppLib.Interop;
using System.Diagnostics;

namespace LlamaCppLib
{
    public class LlmEngine : IDisposable
    {
        private bool _disposed = default;

        private UnmanagedResource _backend = new();
        private UnmanagedResource<nint> _model = new();
        private UnmanagedResource<nint> _context = new();
        private UnmanagedResource<nint> _sampler = new();
        private UnmanagedResource<nint> _vocab = new();
        private UnmanagedResource<llama_batch> _batch = new();

        private LlmEngineOptions _engineOptions = new();
        private LlmModelOptions _modelOptions = new();

        private BlockingQueue<LlmPrompt> _prompts = new();

        private CancellationTokenSource _cancellationTokenSource = new();
        private UnmanagedResource<GCHandle> _cancellationTokenHandle = new();

        private Task? _mainLoop = default;

        public LlmEngine(LlmEngineOptions? engineOptions = default)
        {
            if (engineOptions != default)
                _engineOptions = engineOptions;
        }

        ~LlmEngine() => Dispose(false);

        public IntPtr ModelHandle => _model.Handle;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Managed
                    _StopAsync().Wait();
                }

                // Unmanaged
                _batch.Dispose();
                _sampler.Dispose();
                _context.Dispose();
                _model.Dispose();
                _backend.Dispose();
                _vocab.Dispose();

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void LLamaLogCallback(LLamaLogLevel level, string message)
        {
            Debug.WriteLine(message);
        }

        public unsafe void LoadModel(string modelPath, LlmModelOptions? modelOptions = default, Action<float>? progressCallback = default, bool waitForMainLoop = true)
        {
            if (_model.Created)
                throw new InvalidOperationException("Model already loaded.");

            if (modelOptions != default)
                _modelOptions = modelOptions;

            Native.llama_log_set(LLamaLogCallback, null);

            if (!_backend.Created)
            {
                _backend.Create(() =>
                {
                    llama_backend_init();
                    llama_numa_init(_engineOptions.NumaOptimizations ? ggml_numa_strategy.GGML_NUMA_STRATEGY_DISTRIBUTE : ggml_numa_strategy.GGML_NUMA_STRATEGY_DISABLED);
                }, llama_backend_free);
            }

            //Native.ggml_backend_load_all();
            var libfolder = Native.LlamaCppLibFolder;
            Native.ggml_backend_load_all_from_path(libfolder);
            //var libcuda = Path.Combine(libfolder, "ggml-cuda.dll");
            //IntPtr backend = Native.ggml_backend_load(libcuda);

            var mparams = llama_model_default_params();
            mparams.n_gpu_layers = _modelOptions.GpuLayers;
            mparams.use_mmap = (sbyte)(_modelOptions.UseMemoryMap ? 1 : 0);

            using var progressCallbackHandle = new UnmanagedResource<GCHandle>();
            if (progressCallback != default)
            {
                progressCallbackHandle.Create(() => GCHandle.Alloc(progressCallback), handle => handle.Free());
                mparams.progress_callback = &LlmEngine._ProgressCallback;
                mparams.progress_callback_user_data = GCHandle.ToIntPtr(progressCallbackHandle.Handle).ToPointer();
            }

            _model.Create(() => llama_model_load_from_file(modelPath, mparams), llama_model_free);

            var cparams = llama_context_default_params();
            cparams.n_ctx = (uint)_modelOptions.ContextLength;
            cparams.n_batch = (uint)_modelOptions.BatchSize;
            cparams.n_threads = _modelOptions.ThreadCount;
            cparams.n_threads_batch = _modelOptions.BatchThreadCount;
            cparams.flash_attn = (sbyte)(_modelOptions.UseFlashAttention ? 1 : 0);
            cparams.rope_freq_base = _modelOptions.RopeFrequeceBase;
            cparams.rope_freq_scale = _modelOptions.RopeFrequenceScale;

            _cancellationTokenHandle.Create(() => GCHandle.Alloc(_cancellationTokenSource.Token), handle => handle.Free());
            cparams.abort_callback = &AbortCallback;
            cparams.abort_callback_data = GCHandle.ToIntPtr(_cancellationTokenHandle.Handle).ToPointer();

            _context.Create(() => llama_init_from_model(_model.Handle, cparams), llama_free);

            var sparams = llama_sampler_chain_default_params();
            sparams.no_perf = 0;

            _sampler.Create(() => llama_sampler_chain_init(sparams), llama_sampler_free);

            _batch.Create(() => llama_batch_init((int)llama_n_ctx(_context.Handle), 0, 1), llama_batch_free);

            _vocab.Create(() => Native.llama_model_get_vocab(_model.Handle), Marshal.FreeHGlobal);

            _StartAsync();

            if (waitForMainLoop)
            {
                while (!Loaded)
                {
                    Task.Delay(TimeSpan.FromMilliseconds(100));
                }
            }
        }

        public void UnloadModel()
        {
            _StopAsync().Wait();

            _batch.Dispose();
            _sampler.Dispose();
            _context.Dispose();
            _model.Dispose();
        }

        public bool Loaded => _mainLoop?.Status == TaskStatus.Running || _model.Created;

        public Span<int> Tokenize(string prompt, bool prependBosToken = false, bool processSpecialTokens = false) => 
            llama_tokenize(_vocab.Handle, Encoding.UTF8.GetBytes(prompt), prependBosToken, processSpecialTokens);

        public nint ModelNativeHandle { get => _model.Handle; }
        public nint ContextNativeHandle { get => _context.Handle; }

        public int ContextLength => Loaded ? (int)llama_n_ctx(_context.Handle) : 0;
        public int TrainingContextLength => Loaded ? llama_model_n_ctx_train(_model.Handle) : 0;
        public int LayerCount => Loaded ? llama_model_n_layer(_model.Handle) : 0;

        public LlmPrompt Prompt(
            string promptText,
            SamplingOptions? samplingOptions = default,
            bool? prependBosToken = default,
            bool? processSpecialTokens = default
        )
        {
            var vocab = Native.llama_model_get_vocab(_model.Handle);
            bool _prependBosToken = prependBosToken ??
                llama_vocab_get_add_bos(vocab) > 0 ? true :
                llama_vocab_type(vocab) == _llama_vocab_type.LLAMA_VOCAB_TYPE_SPM;

            var prompt = new LlmPrompt(
                promptText,
                samplingOptions ?? new(),
                _prependBosToken,
                processSpecialTokens ?? true
            );

            _prompts.Enqueue(prompt);
            return prompt;
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        private static unsafe sbyte _ProgressCallback(float progress, void* state)
        {
            var callback = (Action<float>?)GCHandle.FromIntPtr(new(state)).Target;
            callback?.Invoke(progress * 100);
            return true ? 1 : 0;
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        static unsafe sbyte AbortCallback(void* state)
        {
            var cancellationToken = (CancellationToken?)GCHandle.FromIntPtr(new(state)).Target;
            return (sbyte)(cancellationToken?.IsCancellationRequested ?? false ? 1 : 0);
        }

        private void _StartAsync()
        {
            if (_mainLoop != default)
                return;

            _mainLoop = Task.Run(_Run);
        }

        private async Task _StopAsync()
        {
            if (_mainLoop == default)
                return;

            _cancellationTokenSource.Cancel();
            await (_mainLoop ?? Task.CompletedTask).ConfigureAwait(false);
            _cancellationTokenSource = new();

            _mainLoop = default;
        }

        private unsafe void _Run()
        {
            _batch.GetResource(out var batch);

            var sequences = new Slots<LlmSequence>(_engineOptions.MaxParallel);

            //var candidates = new llama_token_data[llama_n_vocab(_model.Handle)];
            var batchView = new llama_batch();

            var cancellationToken = _cancellationTokenSource.Token;
            while (!cancellationToken.IsCancellationRequested)
            {
                // Fill as many sequence slots as possible given pending requests
                while (sequences.HasFreeSlot && _prompts.Any())
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    var prompt = _prompts.Dequeue(cancellationToken);

                    var extraStopTokens = prompt.SamplingOptions.ExtraStopTokens?
                        .Select(tokenText => Tokenize(tokenText, false, true).ToArray())
                        .Where(tokens => tokens.Length == 1)
                        .Select(tokens => tokens.Single())
                        .ToArray();

                    var ctxLength = (int)llama_n_ctx(_context.Handle);
                    var tokens = Tokenize(prompt.PromptText, prompt.PrependBosToken, prompt.ProcessSpecialTokens);

                    var sequence = new LlmSequence(prompt, ctxLength, tokens, extraStopTokens) { T1 = DateTime.Now };
                    var id = sequences.Add(sequence);
                    sequence.Id = id;
                }

                if (cancellationToken.IsCancellationRequested)
                    continue;

                batch.n_tokens = 0;

                foreach (var sequence in sequences)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    for (; sequence.PosBatch < sequence.PosTokens; sequence.PosBatch++)
                        llama_batch_add(ref batch, sequence.Tokens[sequence.PosBatch], sequence.PosBatch, [sequence.Id], false);

                    sequence.PosLogit = batch.n_tokens - 1;
                    batch.logits[sequence.PosLogit] = true ? 1 : 0;
                }

                if (cancellationToken.IsCancellationRequested)
                    continue;

                if (batch.n_tokens == 0)
                {
                    _prompts.WaitForNext(cancellationToken);
                    continue;
                }

                var batchSize = _modelOptions.BatchSize;
                for (var i = 0; i < batch.n_tokens; i += batchSize)
                {
                    var n_tokens = Math.Min(batchSize, batch.n_tokens - i);

                    batchView.n_tokens = n_tokens;
                    batchView.token = batch.token + i;
                    batchView.embd = null;
                    batchView.pos = batch.pos + i;
                    batchView.n_seq_id = batch.n_seq_id + i;
                    batchView.seq_id = batch.seq_id + i;
                    batchView.logits = batch.logits + i;

                    var result = llama_decode(_context.Handle, batchView);

                    if (cancellationToken.IsCancellationRequested)
                        break;

                    if (result != 0)
                    {
                        foreach (var sequence in sequences)
                            sequence.Prompt.TokenChannel.Writer.Complete(new InsufficientMemoryException());

                        sequences.RemoveAll(sequence => true);
                        llama_kv_self_clear(_context.Handle);

                        continue;
                    }

                    foreach (var sequence in sequences)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        if (sequence.PosLogit < i || sequence.PosLogit >= i + n_tokens)
                            continue;

                        // This isn't a fully dynamic sampling chain per sequence, but ideally here we would confirm whether
                        // we need to reset the sampler (i.e. by comparing the current chain with the requested chain).
                        // For now, this is just a static default temperature chain vs greedy sampling based on temperature.
                        llama_sampler_reset(_sampler.Handle);
                        if (sequence.SamplingOptions.Temperature > 0.0f)
                        {
                            // TODO: Add new DRY sampler
                            llama_sampler_chain_add(_sampler.Handle, llama_sampler_init_top_k(sequence.SamplingOptions.TopK));
                            llama_sampler_chain_add(_sampler.Handle, llama_sampler_init_typical(sequence.SamplingOptions.TypicalP, 1));
                            llama_sampler_chain_add(_sampler.Handle, llama_sampler_init_top_p(sequence.SamplingOptions.TopP, 1));
                            llama_sampler_chain_add(_sampler.Handle, llama_sampler_init_min_p(sequence.SamplingOptions.MinP, 1));
                            // TODO: Add new XTC sampler
                            llama_sampler_chain_add(_sampler.Handle, llama_sampler_init_temp(sequence.SamplingOptions.Temperature));
                            llama_sampler_chain_add(_sampler.Handle, llama_sampler_init_dist((uint)sequence.SamplingOptions.Seed));
                        }
                        else
                        {
                            llama_sampler_chain_add(_sampler.Handle, llama_sampler_init_greedy());
                        }

                        if (cancellationToken.IsCancellationRequested)
                            continue;

                        var token = llama_sampler_sample(_sampler.Handle, _context.Handle, sequence.PosLogit - i);

                        if (sequence.T2 == default)
                        {
                            sequence.T2 = DateTime.Now;
                            sequence.Prompt.PromptingSpeed = sequence.PosResponse / ((sequence.T2 - sequence.T1) ?? new()).TotalSeconds;
                        }

                        var stop = false
                            || sequence.PosTokens >= sequence.Tokens.Length - 1
                            || sequence.PosTokens - sequence.PosResponse >= sequence.SamplingOptions.ResponseMaxTokenCount
                            || (sequence.StopTokens?.Contains(token) ?? false)
                            || llama_vocab_is_eog(_vocab.Handle, token);

                        if (!stop)
                        {
                            sequence.Prompt.TokenChannel.Writer.TryWrite(
                                //llama_detokenize(_model.Handle, [token])
                                Interop.llama_token_to_piece(_vocab.Handle, token, true)
                            );

                            sequence.Tokens[sequence.PosTokens++] = token;
                        }

                        if (sequence.Prompt.Cancelled || stop)
                        {
                            sequence.T3 = DateTime.Now;
                            sequence.Prompt.SamplingSpeed = (sequence.PosTokens - sequence.PosResponse - 1) / ((sequence.T3 - sequence.T2) ?? new()).TotalSeconds;

                            if (sequence.Prompt.Cancelled)
                                sequence.Prompt.TokenChannel.Writer.Complete(new OperationCanceledException());
                            else
                                sequence.Prompt.TokenChannel.Writer.Complete();

                            llama_kv_self_seq_rm(_context.Handle, sequence.Id, -1, -1);
                            sequences.Remove(sequence.Id);
                        }
                    }
                }
            }

            if (cancellationToken.IsCancellationRequested)
            {
                // Notify outstanding requests of cancellation
                foreach (var sequence in sequences)
                    sequence.Prompt.TokenChannel.Writer.Complete(new OperationCanceledException());
            }
        }
    }
}

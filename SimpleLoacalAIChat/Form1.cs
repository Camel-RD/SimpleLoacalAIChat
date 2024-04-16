using LlamaCppLib;
using KlonsLIB.Forms;
using KlonsLIB.Misc;
using SimpleLoacalAIChat.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms.Design;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using SimpleLoacalAIChat.Classes;

namespace SimpleLoacalAIChat
{
    public partial class Form1 : MyFormBase0
    {
        public MyData MyData => MyData.St;
        public Form1()
        {
            IsLoading = true;
            MyData.St.MainForm = this;
            InitializeComponent();
            MyData.Settings.ColorThemeId = "dark1";
            SetupMenuRenderer();

            tsConfig.Renderer = MainMenuStrip.Renderer;

            dgvConfig.AutoGenerateColumns = false;

            ConfigList = MyData.MakeConfigItemList();
            bsConfig.DataSource = ConfigList;

            MyData.Settings.ChatConfig = MyData.Config.ChatConfig;
            MyData.Settings.PropertyChanged += Settings_PropertyChanged;

            SetViewState(EFormViewState.Init);
        }

        bool IsLoading = false;

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MySettings.ColorThemeId) ||
                e.PropertyName == nameof(MySettings.FormFont))
            {
                SetupMenuRenderer();
                CheckMyFontAndColors();
                CheckMenuColorTheme();
            }
        }

        BindingList<IConfigItem> ConfigList = null;
        protected override MyColorTheme MyColorTheme => MyData.Settings.ColorTheme;
        SynchronizationContext sync_ctx = null;

        LlmEngine LlmEngine = null;

        private async void Form1_Shown(object sender, EventArgs e)
        {
            sync_ctx = SynchronizationContext.Current;

            CheckMyFontAndColors();
            tabPage2.Scale(ScaleFactor);
            CheckMenuColorTheme();

            ApplySetting();

            var ret = MyData.Config.ChatConfig.CheckLinks();
            if (ret != "Ok")
            {
                ShowWarning(ret);
            }

            LlmEngine = new LlmEngine(new LlmEngineOptions { MaxParallel = 1 });
            if (LlmEngine == null)
            {
                ShowError("Failed to load LlmEngine, missing 'llama.dll'.");
                return;
            }

            var rt = await LoadDefaultPreset();
            IsLoading = false;
            tbPrompt.Focus();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (LoadingModel || PromptGeneratorIsRunning)
            {
                e.Cancel = true;
                return;
            }
            if (LlmEngine != null)
            {
                LlmEngine.Dispose();
                LlmEngine = null;
            }
            SaveSettings();
        }

        private void exTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (exTabControl1.SelectedIndex == 0)
            {
                SaveSettings();
            }
        }

        void ApplySetting()
        {
            if (MyData.Settings.WindowPos == "maximized")
            {
                WindowState = FormWindowState.Maximized;
                return;
            }
            if (MyData.Settings.WindowsRect.Width > 100)
            {
                Size = new Size(MyData.Settings.WindowsRect.Width, MyData.Settings.WindowsRect.Height);
            }
            if (MyData.Settings.WindowsRect.X >= 0)
            {
                Location = new Point(MyData.Settings.WindowsRect.X, MyData.Settings.WindowsRect.Y);
            }
        }

        void SaveSettings()
        {
            if (IsLoading) return;
            MyData.Settings.WindowPos = WindowState == FormWindowState.Maximized ? "maximized" : "normal";
            MyData.Settings.WindowsRect = new BoundsRectange(Left, Top, Width, Height);
            MyData.SaveConfig();
            MyData.SaveSettings();
        }

        public Color RequestColor
        {
            get
            {
                var theme_tag = MyData.Settings.ColorThemeId;
                var ret = theme_tag switch
                {
                    "system" or "blackonwhite" => MyData.Settings.ColorLightRequest,
                    "dark1" or "green" => MyData.Settings.ColorDarkRequest,
                    _ => MyColorTheme.WindowTextColor
                };
                return ret;
            }
        }

        public Color DebugColor
        {
            get
            {
                var theme_tag = MyData.Settings.ColorThemeId;
                var ret = theme_tag switch
                {
                    "system" or "blackonwhite" => MyData.Settings.ColorLightDebug,
                    "dark1" or "green" => MyData.Settings.ColorDarkDebud,
                    _ => MyColorTheme.WindowTextColor
                };
                return ret;
            }
        }

        void SetParagraphSpacing()
        {
            int twips = (int)(Font.Height / 6f * 20f);
            int cur_pos = tbOut.SelectionStart;
            if (tbOut.Text?.Length > 0)
            {
                tbOut.SelectAll();
            }
            tbOut.SetSelectionSpaceBefore(twips);
            tbOut.SetSelectionSpaceAfter(twips);
            if (tbOut.Text?.Length > 0)
            {
                tbOut.SelectionStart = cur_pos;
                tbOut.SelectionLength = 0;
            }
        }

        async Task<bool> LoadDefaultPreset()
        {
            if (LlmEngine == null) return false;
            string preset_name = null;
            if (!Program.StartUpPresetName.IsNOE())
            {
                preset_name = Program.StartUpPresetName;
            }
            else
            {
                if (!MyData.Settings.AutoLoadPreset) return false;
                preset_name = MyData.Settings.AutoLoadPresetName;
            }
            var preset = MyData.Config.ChatConfig.GetConfigPresetByName(preset_name);
            if (preset == null) return false;
            return await ApplyConfigPreset(preset);
        }

        ConfigPreset ActiveConfigPreset = null;
        SamplingParams ActiveSamplingParams = null;
        PromptTemplate ActivePromptTemplate = null;

        public async Task<bool> ApplyConfigPreset(ConfigPreset preset)
        {
            if (LlmEngine == null) return false;

            if (ActiveConfigPreset != null && ActiveConfigPreset.Equals(preset))
                return false;
            var rt = MyData.Config.ChatConfig.CheckConfigPreset(preset);
            if (rt != "Ok")
            {
                ShowError(rt);
                return false;
            }

            var prev_config = ActiveConfigPreset;
            ActiveConfigPreset = preset with { };
            ActivePromptTemplate = ActiveConfigPreset.ChatConfig.GetPromptTemplateByName(ActiveConfigPreset.TemplateName);
            ActiveSamplingParams = ActiveConfigPreset.ChatConfig.GetSamplingParamsName(ActiveConfigPreset.SamplerName);

            PostTask(() => tslPreset.Text = ActiveConfigPreset.Name);

            if (!ShouldReloadModel(prev_config, preset))
                return true;

            var fnm = ActiveConfigPreset.ChatConfig.GetModelFullFileNmae(ActiveConfigPreset.Model);
            if (fnm.IsNOE())
            {
                ShowError($"File not found {ActiveConfigPreset.Model}");
                return false;
            }

            if (LlmEngine.Loaded)
            {
                LlmEngine.UnloadModel();
            }
            var rb = await LoadModel(fnm);
            return rb;
        }

        public bool ShouldReloadModel(ConfigPreset currentpreset, ConfigPreset newpreset)
        {
            if (currentpreset == null) return true;
            if (currentpreset.Model != newpreset.Model) return true;
            if (currentpreset.GpuLayers != newpreset.GpuLayers) return true;
            if (currentpreset.ContextLength != newpreset.ContextLength) return true;
            if (currentpreset.MainGpu != newpreset.MainGpu) return true;
            if (currentpreset.Seed != newpreset.Seed) return true;
            if (currentpreset.BatchSize != newpreset.BatchSize) return true;
            if (currentpreset.BatchThreadCount != newpreset.BatchThreadCount) return true;
            if (currentpreset.ThreadCount != newpreset.ThreadCount) return true;
            return false;
        }

        volatile bool LoadingModel = false;

        public async Task<bool> LoadModel(string model_path)
        {
            if (LoadingModel) return false;
            if (PromptGeneratorIsRunning) return false;
            PostTask(() => SetViewState(EFormViewState.LoadingModel));
            try
            {
                var rt = await Task.Run(() => LoadModelA(model_path));
                if (rt) PostTask(() => SetViewState(EFormViewState.LoadedModel));
                else PostTask(() => SetViewState(EFormViewState.FailedToLoadModel));
                return rt;
            }
            catch (Exception ex)
            {
                PostTask(() => SetViewState(EFormViewState.FailedToLoadModel));
                ShowError(ex.ToString());
                return false;
            }
        }

        public bool LoadModelA(string model_path)
        {
            if (LoadingModel) return false;
            if (PromptGeneratorIsRunning) return false;
            LoadingModel = true;
            try
            {
                LlmEngine.LoadModel(
                    model_path,
                    GetActiveModelOptions(),
                    OnProgress);
                return true;
            }
            catch (Exception ex)
            {
                PostTask(() => ShowError(ex.ToString()));
                return false;
            }
            finally
            {
                LoadingModel = false;
            }
        }

        LlmModelOptions GetActiveModelOptions()
        {
            if (ActiveConfigPreset == null) return new();
            var ret = new LlmModelOptions()
            {
                MainGpu = ActiveConfigPreset.MainGpu,
                GpuLayers = ActiveConfigPreset.GpuLayers,
                Seed = ActiveConfigPreset.Seed,
                BatchSize = ActiveConfigPreset.BatchSize,
                BatchThreadCount = ActiveConfigPreset.BatchThreadCount,
                ThreadCount = ActiveConfigPreset.ThreadCount,
                ContextLength = ActiveConfigPreset.ContextLength
            };
            return ret;
        }

        SamplingOptions GetActiveSamplingOptions()
        {
            if (ActiveSamplingParams == null) return new();
            var ret = new SamplingOptions()
            {
                ExtraStopTokens = ActiveSamplingParams.GetExtraStopTokensArray(),
                Mirostat = ActiveSamplingParams.Mirostat,
                MirostatEta = ActiveSamplingParams.MirostatEta,
                MirostatTau = ActiveSamplingParams.MirostatTau,
                PenaltyFreq = ActiveSamplingParams.PenaltyFreq,
                PenaltyLastN = ActiveSamplingParams.PenaltyLastN,
                PenaltyPresent = ActiveSamplingParams.PenaltyPresent,
                PenaltyRepeat = ActiveSamplingParams.PenaltyRepeat,
                ResponseMaxTokenCount = ActiveSamplingParams.ResponseMaxTokenCount,
                Temperature = ActiveSamplingParams.Temperature,
                TfsZ = ActiveSamplingParams.TfsZ,
                TopK = ActiveSamplingParams.TopK,
                TopP = ActiveSamplingParams.TopP,
                MinP = ActiveSamplingParams.MinP,
                TypicalP = ActiveSamplingParams.TypicalP,
            };
            return ret;
        }

        CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        CancellationToken PromptGeneratorCancellationToken = CancellationToken.None;

        volatile bool PromptGeneratorIsRunning = false;

        public async Task<bool> RunPrompt(string prompt)
        {
            if (LlmEngine == null || string.IsNullOrEmpty(tbPrompt.Text)) return false;
            if (LoadingModel) return false;
            if (PromptGeneratorIsRunning) return false;
            PromptGeneratorIsRunning = true;
            DeferredOutput = "";
            LastOutput = "";
            var tokens_count = LlmEngine.Tokenize(prompt).Length;
            PostTask(() => tslTokenCount.Text = $"Tokens in prompt {tokens_count}/{ActiveConfigPreset.ContextLength}");
            var real_prompt = prompt;
            var options = GetActiveSamplingOptions();
            PostTask(() => SetViewState(EFormViewState.RunningPrompt));
            var sb = new StringBuilder();
            try
            {
                var llm_prompt = LlmEngine.Prompt(
                    real_prompt,
                    options,
                    prependBosToken: true,
                    processSpecialTokens: false
                );

                CancellationTokenSource = new CancellationTokenSource();
                PromptGeneratorCancellationToken = CancellationTokenSource.Token;

                var tt = new TokenEnumerator(llm_prompt, PromptGeneratorCancellationToken);
                await foreach (var token in tt)
                {
                    sb.Append(token);
                    AddStreamingText(token);
                }
                var full_response = sb.ToString().Trim();
                ChatHistory.Add(new ChatHistoryItem(EChatHistoryItemType.Response, full_response));
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex);
                return false;
            }
            finally
            {
                AddStreamingText("\n", true);
                PostTask(() => SetViewState(EFormViewState.FinishedPrompt));
                PromptGeneratorIsRunning = false;
                PromptGeneratorCancellationToken = CancellationToken.None;
            }
        }

        Stopwatch StreamingStopwatch = Stopwatch.StartNew();
        long StreamingLastTs = 0;
        string DeferredOutput = "";
        string LastOutput = "";
        const int DefferedOutputLength = 10;
        void AddStreamingText(string text, bool finished = false)
        {
            text = DeferredOutput + text;
            var elapsed_ms = StreamingStopwatch.ElapsedMilliseconds - StreamingLastTs;
            if (!finished && (elapsed_ms < 1000 || DeferredOutput.Length < 2 * DefferedOutputLength))
            {
                DeferredOutput = text;
                return;
            }
            StreamingLastTs = StreamingStopwatch.ElapsedMilliseconds;

            text = FilterOutEosTokens(text);

            if (finished)
            {
                if (text == "\n" && !LastOutput.IsNOE() && LastOutput.EndsWith("\n\n"))
                    text = "";
                if (text.EndsWith("\n\n\n"))
                    text = text.Substring(0, text.Length - 1);
            }
            else
            {
                int pos = text.Length - DefferedOutputLength;
                if (pos < 0) pos = 0;
                DeferredOutput = text.Substring(pos);
                if (pos == 0) return;
                text = text.Substring(0, pos);
            }

            AddText(text);
            LastOutput = text;
        }

        string FilterOutEosTokens(string text)
        {
            var eos_tokens = ActiveSamplingParams.GetExtraStopTokensArray();
            if (!(eos_tokens?.Length > 0)) return text;
            eos_tokens = eos_tokens.OrderByDescending(x => x.Length).ToArray();
            foreach(var token in eos_tokens)
                text = text.Replace(token, "");
            return text;
        }

        enum EFormViewState
        {
            Init, LoadingModel, LoadedModel, FailedToLoadModel,
            RunningPrompt, FinishedPrompt
        }

        void SetViewState(EFormViewState state)
        {
            bool b1 = state == EFormViewState.LoadedModel || state == EFormViewState.FinishedPrompt;
            tsbAsk.Visible = b1;
            tsbContinue.Visible = b1;
            miDebugPrompt.Visible = b1;
            tsbCancel.Visible = state == EFormViewState.RunningPrompt;
            pbProggress.Visible = state == EFormViewState.LoadingModel;
            var status_text = state switch
            {
                EFormViewState.LoadingModel => "Loading model ...",
                EFormViewState.LoadedModel => "Model loaded",
                EFormViewState.FailedToLoadModel => "Failed to load model",
                EFormViewState.RunningPrompt => "Receiving response ...",
                EFormViewState.FinishedPrompt => "",
                _ => ""
            };
            ShowStatusA(status_text);
        }

        #region ***********  TOOLING  ***********
        void PostTask(Action<object> action, object state = null)
        {
            sync_ctx.Post((o) => action(o), state);
        }

        void PostTask(Action action) =>
            sync_ctx.Post((_) => action(), null);

        void ShowStatus(string text) =>
            PostTask(() => ShowStatusA(text));
        
        void ShowStatusA(string text)
        {
            lbStatus.Text = text;
            lbStatus.Visible = !text.IsNOE();
        }

        void AddText(string text) =>
            PostTask(() => tbOut.AppendText(text));

        public void OnProgress(float progress) =>
            PostTask(() => pbProggress.Value = (int)progress);

        public void AddColoredText(string text, Color color)
        {
            sync_ctx.Post(_ =>
            {
                AddColoredTextA(text, color);
            }, null);
        }

        public void AddColoredTextA(string text, Color color)
        {
            tbOut.SelectionLength = 0;
            tbOut.SelectionStart = tbOut.TextLength;
            tbOut.SelectionColor = color;
            tbOut.AppendText(text);
            tbOut.SelectionLength = 0;
            tbOut.SelectionStart = tbOut.TextLength;
            tbOut.SelectionColor = tbOut.ForeColor;
        }
        public void AddColoredTextLineA(string text, Color color) =>
            AddColoredTextA(text + "\r\n", color);
        public void AddTextLine(string text) =>
            AddText(text + "\r\n");
        public void AddColoredTextLine(string text, Color color) =>
            AddColoredTextLineA(text, color);

        public void ShowInfo(string msg, string title = "Info", Form owner = null)
        {
            MyMessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information, null, owner);
        }
        public void ShowWarning(string msg, string title = "Warning!", Form owner = null)
        {
            MyMessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning, null, owner);
        }
        public void ShowError(string msg, string title = "Error!", Form owner = null)
        {
            MyMessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error, null, owner);
        }

        #endregion

        private void bsConfig_CurrentChanged(object sender, EventArgs e)
        {
            if (bsConfig.Count == 0 || bsConfig.Current == null ||
                bsConfig.Current is not IConfigItem config_item ||
                config_item.ItemType == EConfigItemType.Header)
            {
                tsbConfigLoad.Visible = false;
                pgConfig.SelectedObject = null;
                return;
            }
            var td = new ModelTypeDescriptor(bsConfig.Current);
            pgConfig.SelectedObject = td;
            tsbConfigLoad.Visible = config_item.ItemType == EConfigItemType.Preset;
        }

        private void tsbConfigNew_Click(object sender, EventArgs e)
        {
            if (bsConfig.Count == 0 || bsConfig.Current == null ||
                bsConfig.Current is not IConfigItem config_item)
                return;
            var item_type = config_item.ItemType;
            if (item_type == EConfigItemType.Header && config_item is GroupHeader header_item)
                item_type = header_item.GroupType;
            if (item_type == EConfigItemType.Setting) return;
            var new_config_item = MyData.MakeNewConfigItem(item_type);
            if (new_config_item == null) return;
            int pos = MyData.InsertConfigItem(ConfigList, new_config_item);
            if (pos == -1) return;
            MyData.AddNewConfigItem(new_config_item);
            bsConfig.Position = pos;
        }

        private void tsbConfigCopy_Click(object sender, EventArgs e)
        {
            if (bsConfig.Count == 0 || bsConfig.Current == null ||
                bsConfig.Current is not IConfigItem config_item)
                return;
            var item_type = config_item.ItemType;
            if (item_type == EConfigItemType.Header && config_item is GroupHeader header_item)
                item_type = header_item.GroupType;
            if (item_type == EConfigItemType.Setting) return;
            var new_config_item = MyData.CloneConfigItem(config_item);
            if (new_config_item == null) return;
            int pos = MyData.InsertConfigItem(ConfigList, new_config_item);
            if (pos == -1) return;
            MyData.AddNewConfigItem(new_config_item);
            bsConfig.Position = pos;
        }

        private void tsbConfigDelete_Click(object sender, EventArgs e)
        {
            if (bsConfig.Count == 0 || bsConfig.Current == null ||
                bsConfig.Current is not IConfigItem config_item ||
                config_item.ItemType == EConfigItemType.Header ||
                config_item.ItemType == EConfigItemType.Setting)
                return;
            bsConfig.Remove(config_item);
            MyData.RemoveConfigItem(config_item);
        }

        private async void tsbConfigLoad_Click(object sender, EventArgs e)
        {
            if (bsConfig.Count == 0 || bsConfig.Current == null ||
                bsConfig.Current is not IConfigItem config_item ||
                config_item.ItemType != EConfigItemType.Preset ||
                LlmEngine == null)
                return;
            var cur_configpreset = bsConfig.Current as ConfigPreset;
            await ApplyConfigPreset(cur_configpreset);
        }

        private void tsbConfigSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        public string FormatPrompt(string prompt)
        {
            prompt = prompt.Trim().Replace("\r", "");
            var system_prompt = ActivePromptTemplate.System;
            system_prompt = system_prompt.Replace("\\n", "\n");
            if (!ActiveConfigPreset.SystemPrompt.IsNOE())
                system_prompt = string.Format(system_prompt, ActiveConfigPreset.SystemPrompt);
            var user_prompt = ActivePromptTemplate.Prompt;
            user_prompt = user_prompt.Replace("\\n", "\n");
            user_prompt = string.Format(user_prompt, prompt);
            return system_prompt + user_prompt;
        }

        public string FormatPromptList(List<ChatHistoryItem> list)
        {
            var sb = new StringBuilder();
            var system_prompt = ActivePromptTemplate.System;
            system_prompt = system_prompt.Replace("\\n", "\n");
            if (!ActiveConfigPreset.SystemPrompt.IsNOE())
                system_prompt = string.Format(system_prompt, ActiveConfigPreset.SystemPrompt);
            sb.Append(system_prompt);
            foreach (var item in list)
            {
                if (item.Type == EChatHistoryItemType.Request)
                {
                    var user_prompt = ActivePromptTemplate.Prompt;
                    user_prompt = user_prompt.Replace("\\n", "\n");
                    var prompt = item.Text.Replace("\r", "");
                    user_prompt = string.Format(user_prompt, prompt);
                    sb.Append(user_prompt);
                }
                else
                {
                    var response = ActivePromptTemplate.Response;
                    response = string.Format(response, item.Text);
                    sb.Append(response);
                }
            }
            return sb.ToString();
        }

        private void dgvConfig_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= bsConfig.Count) return;
            var config_item = bsConfig.Current as IConfigItem;
            if (config_item == null) return;
            if (config_item.ItemType == EConfigItemType.Header ||
                config_item.ItemType == EConfigItemType.Setting)
            {
                e.Cancel = true;
                return;
            }
        }

        private void dgvConfig_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= bsConfig.Count) return;
            var config_item = bsConfig[e.RowIndex] as IConfigItem;
            if (config_item == null) return;
            if (config_item.ItemType == EConfigItemType.Header)
            {
                var color = ColorThemeHelper.MakeLighter(BackColor, 0.3f, 0.7f, 2.5f);
                e.CellStyle.BackColor = color;
                e.FormattingApplied = true;
                return;
            }
        }

        public enum EChatHistoryItemType { Request, Response }
        public record ChatHistoryItem(EChatHistoryItemType Type, string Text);

        List<ChatHistoryItem> ChatHistory = new();

        void AddRequestToView(string text)
        {
            var add_atbegining = "";
            if (!tbOut.Text.IsNOE())
            {
                if (!tbOut.Text.EndsWith("\n\n"))
                {
                    if (tbOut.Text.EndsWith("\n")) add_atbegining = "\n";
                    else add_atbegining = "\n\n";
                }
            }
            var add_atend = text.EndsWith("\n") ? "" : "\n";
            AddColoredTextA(add_atbegining + text + add_atend, RequestColor);

        }

        private async void tsbAsk_Click(object sender, EventArgs e)
        {
            var prompt = tbPrompt.Text;
            if (prompt.IsNOE()) return;
            prompt = prompt.Replace("\r", "");
            var formatted_prompt = FormatPrompt(prompt);
            if (formatted_prompt.IsNOE()) return;
            AddRequestToView(prompt);
            ChatHistory.Clear();
            ChatHistory.Add(new ChatHistoryItem(EChatHistoryItemType.Request, prompt));
            await RunPrompt(formatted_prompt);
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            if (!PromptGeneratorIsRunning) return;
            CancellationTokenSource.Cancel();
        }

        private async void tsbContinue_Click(object sender, EventArgs e)
        {
            var prompt = tbPrompt.Text;
            if (prompt.IsNOE()) return;
            prompt = prompt.Replace("\r", "");
            ChatHistory.Add(new ChatHistoryItem(EChatHistoryItemType.Request, prompt));
            var formatted_prompt = FormatPromptList(ChatHistory);
            if (formatted_prompt.IsNOE()) return;
            AddRequestToView(prompt);
            await RunPrompt(formatted_prompt);
        }

        private void miDebugPrompt_Click(object sender, EventArgs e)
        {
            var list = new List<ChatHistoryItem>(ChatHistory);
            var prompt = tbPrompt.Text;
            if (!prompt.IsNOE())
            {
                prompt = prompt.Replace("\r", "");
                list.Add(new ChatHistoryItem(EChatHistoryItemType.Request, prompt));
            }
            var formatted_prompt = FormatPromptList(list);
            if (formatted_prompt.IsNOE()) return;

            var add_atbegining = !tbOut.Text.IsNOE() && !tbOut.Text.EndsWith("\n") ? "\n" : "";
            var add_atend = prompt.EndsWith("\n") ? "" : "\n";
            AddColoredTextA(add_atbegining + formatted_prompt + add_atend, DebugColor);
        }

    }
}

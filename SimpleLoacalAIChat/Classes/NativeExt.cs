using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LlamaCppLib;

namespace SimpleLoacalAIChat
{
    public static unsafe partial class NativeExt
    {
        internal const string libraryName = "llama";
        private const string LibName = $"{nameof(LlamaCppLib)}/llama";

        private static int llama_model_meta_key_by_index(IntPtr model, int index, Span<byte> dest)
        {
            unsafe
            {
                fixed (byte* destPtr = dest)
                {
                    return llama_model_meta_key_by_index_native(model, index, destPtr, dest.Length);
                }
            }

            [DllImport(libraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "llama_model_meta_key_by_index")]
            static extern unsafe int llama_model_meta_key_by_index_native(IntPtr model, int index, byte* buf, long buf_size);
        }

        private static int llama_model_meta_val_str_by_index(IntPtr model, int index, Span<byte> dest)
        {
            unsafe
            {
                fixed (byte* destPtr = dest)
                {
                    return llama_model_meta_val_str_by_index_native(model, index, destPtr, dest.Length);
                }
            }

            [DllImport(libraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "llama_model_meta_val_str_by_index")]
            static extern unsafe int llama_model_meta_val_str_by_index_native(IntPtr model, int index, byte* buf, long buf_size);
        }

        public static Memory<byte>? MetadataKeyByIndex(IntPtr model, int index)
        {
            // Check if the key exists, without getting any bytes of data
            var keyLength = llama_model_meta_key_by_index(model, index, Array.Empty<byte>());
            if (keyLength < 0)
                return null;

            // get a buffer large enough to hold it
            var buffer = new byte[keyLength + 1];
            keyLength = llama_model_meta_key_by_index(model, index, buffer);
            Debug.Assert(keyLength >= 0);

            return buffer.AsMemory().Slice(0, keyLength);
        }

        public static Memory<byte>? MetadataValueByIndex(IntPtr model, int index)
        {
            // Check if the key exists, without getting any bytes of data
            var valueLength = llama_model_meta_val_str_by_index(model, index, Array.Empty<byte>());
            if (valueLength < 0)
                return null;

            // get a buffer large enough to hold it
            var buffer = new byte[valueLength + 1];
            valueLength = llama_model_meta_val_str_by_index(model, index, buffer);
            Debug.Assert(valueLength >= 0);

            return buffer.AsMemory().Slice(0, valueLength);
        }

        [DllImport(libraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int llama_model_meta_count(IntPtr model);

        public static IReadOnlyDictionary<string, string> ReadMetadata(IntPtr model)
        {
            var result = new Dictionary<string, string>();

            for (var i = 0; i < llama_model_meta_count(model); i++)
            {
                var keyBytes = MetadataKeyByIndex(model, i);
                if (keyBytes == null)
                    continue;
                var key = Encoding.UTF8.GetStringFromSpan(keyBytes.Value.Span);

                var valBytes = MetadataValueByIndex(model, i);
                if (valBytes == null)
                    continue;
                var val = Encoding.UTF8.GetStringFromSpan(valBytes.Value.Span);

                result[key] = val;
            }

            return result;
        }

        internal static string GetStringFromSpan(this Encoding encoding, ReadOnlySpan<byte> bytes)
        {
            unsafe
            {
                fixed (byte* bytesPtr = bytes)
                {
                    return encoding.GetString(bytesPtr, bytes.Length);
                }
            }
        }

    }


}

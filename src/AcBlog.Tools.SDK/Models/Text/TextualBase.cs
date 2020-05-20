using System;
using System.Data;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;

namespace AcBlog.Tools.SDK.Models.Text
{
    public abstract class TextualBase<T, TMeta> where TMeta : class
    {
        static ISerializer YamlSerializer { get; } = new SerializerBuilder().Build();

        static IDeserializer YamlDeserializer { get; } = new DeserializerBuilder().Build();

        const string MetaSplitter = "---";

        protected abstract T CreateInitialData();

        protected virtual string FormatMetadata(TMeta metadata)
        {
            return YamlSerializer.Serialize(metadata!).Trim(new char[] { '\r', '\n' });
        }

        protected virtual TMeta ParseMetadata(string metastr)
        {
            return YamlDeserializer.Deserialize<TMeta>(metastr);
        }

        protected virtual string FormatData(T data) => string.Empty;

        protected virtual TMeta? GetMetadata(T data) => null;

        protected virtual void SetMetadata(T data, TMeta meta) { }

        protected virtual void SetData(T data, string datastr) { }

        public virtual string Format(T data)
        {
            StringBuilder sb = new StringBuilder();
            var meta = GetMetadata(data);
            if (meta != null)
            {
                sb.Append($"{MetaSplitter}\n{FormatMetadata(meta)}\n{MetaSplitter}\n");
            }
            sb.Append(FormatData(data));
            return sb.ToString();
        }

        public virtual T Parse(string rawText)
        {
            T result = CreateInitialData();

            var lines = rawText.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n");
            int contentBg = 0;
            if (lines.Length > 0)
            {
                if (lines[0] == MetaSplitter)
                {
                    int l = 1, r = 1;
                    for (; r < lines.Length; r++)
                    {
                        if (lines[r] == lines[0])
                            break;
                    }
                    var metastr = string.Join('\n', lines[l..r]);
                    contentBg = r;
                    try
                    {
                        var meta = ParseMetadata(metastr);
                        SetMetadata(result, meta);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to set metadata.", ex);
                    }
                }
            }
            if (contentBg + 1 < lines.Length)
            {
                var datastr = string.Join('\n', lines[(contentBg + 1)..]);
                SetData(result, datastr);
            }
            else
            {
                SetData(result, string.Empty);
            }

            return result;
        }
    }
}

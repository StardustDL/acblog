using System;
using System.Data;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;

namespace AcBlog.Tools.Sdk.Models.Text
{
    public static class ObjectTextual
    {
        static ISerializer YamlSerializer { get; } = new SerializerBuilder().Build();

        static IDeserializer YamlDeserializer { get; } = new DeserializerBuilder().Build();


        public const string MetaSplitter = "---";

        public static string Format<TMeta>(TMeta metadata, string data)
        {
            StringBuilder sb = new StringBuilder();
            if (metadata is not null)
            {
                string metastr = YamlSerializer.Serialize(metadata).Trim(new char[] { '\r', '\n' });
                sb.Append($"{MetaSplitter}\n{metastr}\n{MetaSplitter}\n");
            }
            sb.Append(data);
            return sb.ToString();
        }

        public static (TMeta, string) Parse<TMeta>(string rawText) where TMeta : new()
        {
            TMeta meta = new TMeta();

            var lines = rawText.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n");
            int contentBg = 0;

            if (lines.Length > 0)
            {
                if (lines[0] is MetaSplitter)
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
                        meta = YamlDeserializer.Deserialize<TMeta>(metastr);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to parse metadata.", ex);
                    }
                }
                else
                {
                    contentBg = -1;
                }
            }
            if (contentBg + 1 < lines.Length)
            {
                var datastr = string.Join('\n', lines[(contentBg + 1)..]);
                return (meta, datastr);
            }
            else
            {
                return (meta, string.Empty);
            }
        }
    }
}

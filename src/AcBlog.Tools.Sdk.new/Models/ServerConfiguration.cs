using System;

namespace AcBlog.Tools.Sdk.Models
{
    public class ServerConfiguration
    {
        public ServerConfiguration() : this(new Uri("https://localhost"), true)
        {
        }

        public ServerConfiguration(Uri uri, bool isStatic)
        {
            Uri = uri;
            IsStatic = isStatic;
        }

        public Uri Uri { get; set; }

        public bool IsStatic { get; set; } = false;
    }
}

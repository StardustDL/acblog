using System;

namespace AcBlog.Tools.SDK.Models
{
    public class ServerConfiguration
    {
        public ServerConfiguration(Uri uri, bool isStatic)
        {
            Uri = uri;
            IsStatic = isStatic;
        }

        public Uri Uri { get; set; }

        public bool IsStatic { get; set; } = false;
    }
}

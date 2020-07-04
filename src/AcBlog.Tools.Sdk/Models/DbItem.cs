using System;

namespace AcBlog.Tools.Sdk.Models
{
    public class DbItem
    {
        public string RemoteHash { get; set; } = string.Empty;

        public DateTimeOffset LastUpdateTime { get; set; }
    }
}

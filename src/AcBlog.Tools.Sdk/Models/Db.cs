using System;

namespace AcBlog.Tools.Sdk.Models
{
    public class DB
    {
        public DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.MinValue;
    }
}

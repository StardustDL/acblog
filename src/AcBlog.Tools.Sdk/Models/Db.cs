using System;
using System.Collections.Generic;

namespace AcBlog.Tools.Sdk.Models
{
    public class DB
    {
        public DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.MinValue;
    }
}

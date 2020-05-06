using System;
using System.Collections.Generic;

namespace AcBlog.Tool.StaticGenerator
{
    internal class PostMetadata
    {
        public string title { get; set; }

        public DateTime? date { get; set; }

        public IList<string> keywords { get; set; }

        public IList<string> category { get; set; }

        public string password { get; set; }
    }
}

using AcBlog.Data.Models;
using System;
using System.Collections.Generic;

namespace AcBlog.Tools.SDK.Models.Text
{
    public class PostTextual : TextualBase<Post, PostTextual.Metadata>
    {
        public class Metadata
        {
            public string title { get; set; } = string.Empty;

            public DateTime? date { get; set; }

            public IList<string> keywords { get; set; } = Array.Empty<string>();

            public IList<string> category { get; set; } = Array.Empty<string>();

            public string password { get; set; } = string.Empty;

            public string type { get; set; } = string.Empty;
        }

        protected override string FormatData(Post data) => data.Content.Raw;

        protected override void SetData(Post data, string datastr) => data.Content = new Document
        {
            Raw = datastr
        };

        public Post InitialData { get; set; } = new Post();

        protected override Post CreateInitialData() => InitialData;
    }
}

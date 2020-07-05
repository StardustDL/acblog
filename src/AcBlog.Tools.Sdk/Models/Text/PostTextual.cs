using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AcBlog.Tools.Sdk.Models.Text
{
    public class PostTextual : TextualBase<Post, PostTextual.Metadata>
    {
        public class Metadata
        {
            public string id { get; set; } = string.Empty;

            public string author { get; set; } = string.Empty;

            public string title { get; set; } = string.Empty;

            public string creationTime { get; set; } = string.Empty;

            public string modificationTime { get; set; } = string.Empty;

            public string[] keywords { get; set; } = Array.Empty<string>();

            public string[] category { get; set; } = Array.Empty<string>();

            public string type { get; set; } = string.Empty;
        }

        protected override Metadata? GetMetadata(Post data) => new Metadata
        {
            id = data.Id,
            title = data.Title,
            author = data.AuthorId,
            creationTime = data.CreationTime.ToString(),
            modificationTime = data.ModificationTime.ToString(),
            category = data.Category.ToArray(),
            keywords = data.Keywords.ToArray(),
            type = Enum.GetName(typeof(PostType), data.Type)!,
        };

        protected override void SetMetadata(Post data, Metadata meta)
        {
            data.Id = meta.id;
            data.Title = meta.title;
            data.Category = new Category(meta.category);
            data.Keywords = new Keyword(meta.keywords);
            data.CreationTime = DateTimeOffset.Parse(meta.creationTime);
            data.Type = Enum.Parse<PostType>(meta.type);
            data.ModificationTime = DateTimeOffset.Parse(meta.modificationTime);
            data.AuthorId = meta.author;
        }

        protected override string FormatData(Post data) => data.Content.Raw;

        protected override void SetData(Post data, string datastr) => data.Content = new Document
        {
            Raw = datastr
        };
    }
}

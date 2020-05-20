using AcBlog.Data.Models;
using System;
using System.Collections.Generic;

namespace AcBlog.Tools.SDK.Models.Text
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

            public string category { get; set; } = string.Empty;

            public string type { get; set; } = string.Empty;
        }

        protected override Metadata? GetMetadata(Post data) => new Metadata
        {
            id = data.Id,
            title = data.Title,
            author = data.AuthorId,
            creationTime = data.CreationTime.ToString(),
            modificationTime = data.ModificationTime.ToString(),
            category = data.CategoryId,
            keywords = data.KeywordIds,
            type = Enum.GetName(typeof(PostType), data.Type)!,
        };

        protected override void SetMetadata(Post data, Metadata meta)
        {
            data.Id = meta.id;
            data.Title = meta.title;
            data.CategoryId = meta.category;
            data.KeywordIds = meta.keywords;
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

        public Post InitialData { get; set; } = new Post();

        protected override Post CreateInitialData() => InitialData;
    }
}

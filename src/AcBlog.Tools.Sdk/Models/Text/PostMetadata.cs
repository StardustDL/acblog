using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AcBlog.Tools.Sdk.Models.Text
{
    public class PostMetadata
    {
        public string id { get; set; } = string.Empty;

        public string author { get; set; } = string.Empty;

        public string title { get; set; } = string.Empty;

        public string creationTime { get; set; } = DateTimeOffset.MinValue.ToString();

        public string modificationTime { get; set; } = DateTimeOffset.MinValue.ToString();

        public string[] keywords { get; set; } = Array.Empty<string>();

        public string[] category { get; set; } = Array.Empty<string>();

        public string type { get; set; } = Enum.GetName(typeof(PostType), PostType.Article)?.ToLowerInvariant() ?? "article";

        public PostMetadata() { }

        public PostMetadata(Post data)
        {
            id = data.Id;
            title = data.Title;
            author = data.AuthorId;
            creationTime = data.CreationTime.ToString();
            modificationTime = data.ModificationTime.ToString();
            category = data.Category.Items.ToArray();
            keywords = data.Keywords.Items.ToArray();
            type = Enum.GetName(typeof(PostType), data.Type)?.ToLowerInvariant() ?? string.Empty;
        }

        public void ApplyTo(Post data)
        {
            data.Id = id;
            data.Title = title;
            data.Category = new Category(category);
            data.Keywords = new Keyword(keywords);
            data.CreationTime = DateTimeOffset.Parse(creationTime);
            data.Type = Enum.Parse<PostType>(type, true);
            data.ModificationTime = DateTimeOffset.Parse(modificationTime);
            data.AuthorId = author;
        }
    }
}

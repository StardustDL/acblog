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

        public string creationTime { get; set; } = string.Empty;

        public string modificationTime { get; set; } = string.Empty;

        public string[] keywords { get; set; } = Array.Empty<string>();

        public string[] category { get; set; } = Array.Empty<string>();

        public string type { get; set; } = Enum.GetName(typeof(PostType), PostType.Article)?.ToLowerInvariant() ?? "article";

        public string password { get; set; } = string.Empty;

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
            if(DateTimeOffset.TryParse(creationTime,out var _creationTime))
            {
                data.CreationTime = _creationTime;
            }
            if (DateTimeOffset.TryParse(modificationTime, out var _modificationTime))
            {
                data.ModificationTime = _modificationTime;
            }
            data.Type = Enum.Parse<PostType>(type, true);
            data.AuthorId = author;
        }
    }
}

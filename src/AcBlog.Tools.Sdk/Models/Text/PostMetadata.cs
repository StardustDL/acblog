using AcBlog.Data.Models;
using System;
using System.Linq;

namespace AcBlog.Tools.Sdk.Models.Text
{
    public class PostMetadata : MetadataBase<Post>
    {
        public string? id { get; set; } = null;

        public string? author { get; set; } = null;

        public string? title { get; set; } = null;

        public string? creationTime { get; set; } = null;

        public string? modificationTime { get; set; } = null;

        public string[]? keywords { get; set; } = null;

        public string[]? category { get; set; } = null;

        public string? type { get; set; } = null;

        public string? password { get; set; } = null;

        public PostMetadata() { }

        public PostMetadata(Post data)
        {
            id = data.Id;
            title = data.Title;
            author = data.Author;
            creationTime = data.CreationTime.ToString();
            modificationTime = data.ModificationTime.ToString();
            category = data.Category.Items.ToArray();
            keywords = data.Keywords.Items.ToArray();
            type = Enum.GetName(typeof(PostType), data.Type)?.ToLowerInvariant() ?? string.Empty;
        }

        public override void ApplyTo(Post data)
        {
            if (id != null)
                data.Id = id;
            if (title != null)
                data.Title = title;
            if (category != null)
                data.Category = new Category(category);
            if (keywords != null)
                data.Keywords = new Keyword(keywords);
            if (DateTimeOffset.TryParse(creationTime, out var _creationTime))
            {
                data.CreationTime = _creationTime;
            }
            if (DateTimeOffset.TryParse(modificationTime, out var _modificationTime))
            {
                data.ModificationTime = _modificationTime;
            }
            if (type != null)
                data.Type = Enum.Parse<PostType>(type, true);
            if (author != null)
                data.Author = author;
        }
    }
}

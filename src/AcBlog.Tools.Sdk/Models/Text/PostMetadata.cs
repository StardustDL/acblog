using AcBlog.Data.Models;
using System;
using System.Linq;

#pragma warning disable IDE1006 // 命名样式
#pragma warning disable CA1805 // 避免进行不必要的初始化

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

        public override Post ApplyTo(Post data)
        {
            if (id is not null)
                data = data with { Id = id };
            if (title is not null)
                data = data with { Title = title };
            if (category is not null)
                data = data with { Category = new Category { Items = category } };
            if (keywords is not null)
                data = data with { Keywords = new Keyword { Items = keywords } };
            if (DateTimeOffset.TryParse(creationTime, out var _creationTime))
            {
                data = data with { CreationTime = _creationTime };
            }
            if (DateTimeOffset.TryParse(modificationTime, out var _modificationTime))
            {
                data = data with { ModificationTime = _modificationTime };
            }
            if (type is not null)
                data = data with { Type = Enum.Parse<PostType>(type, true) };
            if (author is not null)
                data = data with { Author = author };
            return data;
        }
    }
}

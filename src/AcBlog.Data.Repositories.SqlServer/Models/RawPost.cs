using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Builders;
using System;
using System.Text.Json;

namespace AcBlog.Data.Repositories.SqlServer.Models
{
    public class RawPost : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public PostType Type { get; set; } = PostType.Article;

        public string Author { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Keywords { get; set; } = string.Empty;

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public static RawPost From(Post value)
        {
            return new RawPost
            {
                Id = value.Id,
                Author = value.Author,
                Category = value.Category.ToString(),
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
                Title = value.Title,
                Type = value.Type,
                Keywords = value.Keywords.ToString(),
                Content = JsonSerializer.Serialize(value.Content),
            };
        }

        public static Post To(RawPost value)
        {
            return new Post
            {
                Id = value.Id,
                Author = value.Author,
                Category = CategoryBuilder.FromString(value.Category),
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
                Title = value.Title,
                Type = value.Type,
                Keywords = KeywordBuilder.FromString(value.Keywords),
                Content = JsonSerializer.Deserialize<Document>(value.Content),
            };
        }
    }
}

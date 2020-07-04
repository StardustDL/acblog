using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace AcBlog.Data.Repositories.SQLServer.Models
{
    public class RawPost
    {
        public string Id { get; set; } = string.Empty;

        public PostType Type { get; set; } = PostType.Article;

        public string AuthorId { get; set; } = string.Empty;

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
                AuthorId = value.AuthorId,
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
                AuthorId = value.AuthorId,
                Category = AcBlog.Data.Models.Category.Parse(value.Category),
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
                Title = value.Title,
                Type = value.Type,
                Keywords = AcBlog.Data.Models.Keyword.Parse(value.Keywords),
                Content = JsonSerializer.Deserialize<Document>(value.Content),
            };
        }
    }
}

using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Data.Repositories.SQLServer.Models
{
    public class PostData
    {
        public string Id { get; set; } = string.Empty;

        public PostType Type { get; set; } = PostType.Article;

        public string AuthorId { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string CategoryId { get; set; } = string.Empty;

        public string KeywordIds { get; set; } = string.Empty;

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public static PostData From(Post value)
        {
            return new PostData
            {
                Id = value.Id,
                AuthorId = value.AuthorId,
                CategoryId = value.CategoryId,
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
                Title = value.Title,
                Type = value.Type,
                KeywordIds = string.Join("$%$", value.KeywordIds),
                Content = value.Content.Raw,
            };
        }

        public static Post To(PostData value)
        {
            return new Post
            {
                Id = value.Id,
                AuthorId = value.AuthorId,
                CategoryId = value.CategoryId,
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
                Title = value.Title,
                Type = value.Type,
                KeywordIds = value.KeywordIds.Split("$%$"),
                Content = new Document
                {
                    Raw = value.Content
                },
            };
        }
    }
}

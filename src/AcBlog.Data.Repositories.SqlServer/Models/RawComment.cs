using AcBlog.Data.Models;
using System;

namespace AcBlog.Data.Repositories.SqlServer.Models
{
    public class RawComment : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public string Extra { get; set; } = string.Empty;

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public string Uri { get; set; } = string.Empty;

        public static RawComment From(Comment value)
        {
            return new RawComment
            {
                Id = value.Id,
                Content = value.Content,
                Author = value.Author,
                Email = value.Email,
                Extra = value.Extra,
                Link = value.Link,
                Uri = value.Uri,
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
            };
        }

        public static Comment To(RawComment value)
        {
            return new Comment
            {
                Id = value.Id,
                Content = value.Content,
                Author = value.Author,
                Email = value.Email,
                Extra = value.Extra,
                Link = value.Link,
                Uri = value.Uri,
                CreationTime = value.CreationTime,
                ModificationTime = value.ModificationTime,
            };
        }
    }
}

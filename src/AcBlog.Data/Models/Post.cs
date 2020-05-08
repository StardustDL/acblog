using System;

namespace AcBlog.Data.Models
{
    public class Post
    {
        public string Id { get; set; } = string.Empty;

        public PostType Type { get; set; } = PostType.Article;

        public string AuthorId { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string[] Category { get; set; } = Array.Empty<string>();

        public string[] Keywords { get; set; } = Array.Empty<string>();

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }
    }
}

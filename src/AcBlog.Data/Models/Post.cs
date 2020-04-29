using System;

namespace AcBlog.Data.Models
{
    public class Post
    {
        public string Id { get; set; } = string.Empty;

        public string AuthorId { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }
    }
}

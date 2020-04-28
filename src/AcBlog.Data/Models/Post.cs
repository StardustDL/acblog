using System;

namespace AcBlog.Data.Models
{
    public class Post
    {
        public string Id { get; set; }

        public string AuthorId { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }
    }
}

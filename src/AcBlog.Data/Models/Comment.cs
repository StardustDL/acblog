using System;

namespace AcBlog.Data.Models
{
    public class Comment : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public string Uri { get; set; } = string.Empty;
    }
}

using System;

namespace AcBlog.Data.Models
{
    public record Comment : RHasId<string>
    {
        public string Content { get; init; } = string.Empty;

        public string Author { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public string Link { get; init; } = string.Empty;

        public string Extra { get; init; } = string.Empty;

        public DateTimeOffset CreationTime { get; init; }

        public DateTimeOffset ModificationTime { get; init; }

        public string Uri { get; init; } = string.Empty;
    }
}

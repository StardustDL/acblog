using System;

namespace AcBlog.Data.Models
{
    public record Statistic : RHasId<string>
    {
        public string Category { get; init; } = string.Empty;

        public string Uri { get; init; } = string.Empty;

        public string Payload { get; init; } = string.Empty;

        public DateTimeOffset CreationTime { get; init; }

        public DateTimeOffset ModificationTime { get; init; }
    }
}

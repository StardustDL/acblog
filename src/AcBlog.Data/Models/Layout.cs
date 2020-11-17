using System;

namespace AcBlog.Data.Models
{
    public record Layout : RHasId<string>
    {
        public string Template { get; init; } = string.Empty;

        public DateTimeOffset CreationTime { get; init; }

        public DateTimeOffset ModificationTime { get; init; }
    }
}

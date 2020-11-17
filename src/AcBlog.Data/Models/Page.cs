using System;

namespace AcBlog.Data.Models
{
    public record Page : RHasId<string>
    {
        public string Layout { get; init; } = string.Empty;

        public string Content { get; init; } = string.Empty;

        public string Route { get; init; } = string.Empty;

        public string Title { get; init; } = string.Empty;

        public Feature Features { get; init; } = Feature.Empty;

        public PropertyCollection Properties { get; init; } = new PropertyCollection();

        public DateTimeOffset CreationTime { get; init; }

        public DateTimeOffset ModificationTime { get; init; }
    }
}

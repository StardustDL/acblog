using System;

namespace AcBlog.Data.Models
{
    public class Page : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Layout { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Route { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public Feature Features { get; set; } = Feature.Empty;

        public PropertyCollection Properties { get; set; } = new PropertyCollection();

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }
    }
}

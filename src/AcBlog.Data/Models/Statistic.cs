using System;

namespace AcBlog.Data.Models
{
    public class Statistic : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Uri { get; set; } = string.Empty;

        public string Payload { get; set; } = string.Empty;

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }
    }
}

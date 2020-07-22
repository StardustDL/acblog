using System;
using System.Collections.Generic;

namespace AcBlog.Data.Models
{
    public class Page : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Layout { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public string Route { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }
    }
}

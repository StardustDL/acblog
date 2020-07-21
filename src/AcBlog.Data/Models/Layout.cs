using System;

namespace AcBlog.Data.Models
{
    public class Layout : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Template { get; set; } = string.Empty;

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }
    }
}

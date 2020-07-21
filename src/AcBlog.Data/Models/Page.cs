using System;
using System.Collections.Generic;

namespace AcBlog.Data.Models
{
    public class Page : IHasId<string>
    {
        private string _route = string.Empty;

        public string Id { get; set; } = string.Empty;

        public string Layout { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Route
        {
            get => _route; set
            {
                _route = value.Trim('/');
            }
        }

        public string Title { get; set; } = string.Empty;

        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }
    }
}

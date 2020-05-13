using System;
using System.Linq;

namespace AcBlog.Data.Models
{
    public class Post : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public PostType Type { get; set; } = PostType.Article;

        public string AuthorId { get; set; } = string.Empty;

        public Document Content { get; set; } = new Document();

        public string Title { get; set; } = string.Empty;

        public string CategoryId { get; set; } = string.Empty;

        public string[] KeywordIds { get; set; } = Array.Empty<string>();

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }
    }
}

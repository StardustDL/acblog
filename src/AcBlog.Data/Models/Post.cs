using AcBlog.Data.Documents;
using System;
using System.Collections.Generic;
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

        public Category Category { get; set; } = Category.Empty;

        public Keyword KeywordIds { get; set; } = Keyword.Empty;

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }
    }
}

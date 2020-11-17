using AcBlog.Data.Documents;
using System;

namespace AcBlog.Data.Models
{
    public record Post : RHasId<string>
    {
        public PostType Type { get; init; } = PostType.Article;

        public string Author { get; init; } = string.Empty;

        public Document Content { get; init; } = new Document();

        public string Title { get; init; } = string.Empty;

        public Category Category { get; init; } = Category.Empty;

        public Keyword Keywords { get; init; } = Keyword.Empty;

        public DateTimeOffset CreationTime { get; init; }

        public DateTimeOffset ModificationTime { get; init; }
    }
}

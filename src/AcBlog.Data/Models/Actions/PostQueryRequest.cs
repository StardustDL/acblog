namespace AcBlog.Data.Models.Actions
{
    public record PostQueryRequest : QueryRequest
    {
        public PostType? Type { get; init; }

        public string Author { get; init; } = string.Empty;

        public string Content { get; init; } = string.Empty;

        public string Title { get; init; } = string.Empty;

        public Keyword? Keywords { get; init; }

        public Category? Category { get; init; }

        public QueryTimeOrder Order { get; init; } = QueryTimeOrder.None;
    }
}

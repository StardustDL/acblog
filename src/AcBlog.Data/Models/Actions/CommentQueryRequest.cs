namespace AcBlog.Data.Models.Actions
{
    public record CommentQueryRequest : QueryRequest
    {
        public string Content { get; init; } = string.Empty;

        public string Author { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public string Link { get; init; } = string.Empty;

        public string Uri { get; init; } = string.Empty;

        public QueryTimeOrder Order { get; init; } = QueryTimeOrder.None;
    }
}

namespace AcBlog.Data.Models.Actions
{
    public record StatisticQueryRequest : QueryRequest
    {
        public string Category { get; init; } = string.Empty;

        public string Uri { get; init; } = string.Empty;

        public string Payload { get; init; } = string.Empty;

        public QueryTimeOrder Order { get; init; } = QueryTimeOrder.None;
    }
}

namespace AcBlog.Data.Models.Actions
{
    public record PageQueryRequest : QueryRequest
    {
        public string Route { get; init; } = string.Empty;

        public QueryTimeOrder Order { get; init; } = QueryTimeOrder.None;
    }
}

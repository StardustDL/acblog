namespace AcBlog.Data.Models.Actions
{
    public record LayoutQueryRequest : QueryRequest
    {
        public QueryTimeOrder Order { get; init; } = QueryTimeOrder.None;
    }
}

namespace AcBlog.Data.Models.Actions
{
    public class PageQueryRequest : QueryRequest
    {
        public string Route { get; set; } = string.Empty;

        public QueryTimeOrder Order { get; set; } = QueryTimeOrder.None;
    }
}

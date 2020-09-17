namespace AcBlog.Data.Models.Actions
{
    public class LayoutQueryRequest : QueryRequest
    {
        public QueryTimeOrder Order { get; set; } = QueryTimeOrder.None;
    }
}

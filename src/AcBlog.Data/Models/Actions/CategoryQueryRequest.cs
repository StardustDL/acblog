namespace AcBlog.Data.Models.Actions
{
    public class CategoryQueryRequest : QueryRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}

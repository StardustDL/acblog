namespace AcBlog.Data.Models.Actions
{
    public class QueryRequest
    {
        public Pagination? Pagination { get; set; } = null;

        public string Term { get; set; } = string.Empty;
    }
}

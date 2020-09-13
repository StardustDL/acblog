namespace AcBlog.Data.Models.Actions
{
    public class QueryRequest
    {
        public Pagination? Pagination { get; set; }

        public string Term { get; set; } = string.Empty;
    }
}

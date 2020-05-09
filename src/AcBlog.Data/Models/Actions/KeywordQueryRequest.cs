namespace AcBlog.Data.Models.Actions
{
    public class KeywordQueryRequest
    {
        public string Name { get; set; } = string.Empty;

        public Pagination? Pagination { get; set; } = null;
    }
}

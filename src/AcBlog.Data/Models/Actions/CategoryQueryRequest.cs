namespace AcBlog.Data.Models.Actions
{
    public class CategoryQueryRequest
    {
        public string Name { get; set; } = string.Empty;

        public Pagination? Pagination { get; set; } = null;
    }
}

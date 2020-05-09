namespace AcBlog.Data.Models
{
    public class Category
    {
        public string ParentId { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}

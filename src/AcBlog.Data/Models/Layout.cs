namespace AcBlog.Data.Models
{
    public class Layout : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Template { get; set; } = string.Empty;
    }
}

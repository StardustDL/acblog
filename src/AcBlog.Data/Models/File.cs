namespace AcBlog.Data.Models
{
    public class File : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Uri { get; set; } = string.Empty;
    }
}

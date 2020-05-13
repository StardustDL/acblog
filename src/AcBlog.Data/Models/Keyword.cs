namespace AcBlog.Data.Models
{
    public class Keyword : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}

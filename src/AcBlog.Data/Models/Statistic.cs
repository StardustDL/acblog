namespace AcBlog.Data.Models
{
    public class Statistic : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Uri { get; set; } = string.Empty;

        public string Data { get; set; } = string.Empty;
    }
}

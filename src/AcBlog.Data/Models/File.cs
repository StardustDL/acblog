namespace AcBlog.Data.Models
{
    public record File : RHasId<string>
    {
        public string Uri { get; init; } = string.Empty;
    }
}

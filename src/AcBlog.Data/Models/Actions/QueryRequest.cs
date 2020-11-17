namespace AcBlog.Data.Models.Actions
{
    public record QueryRequest
    {
        public Pagination? Pagination { get; init; }

        public string Term { get; init; } = string.Empty;
    }
}

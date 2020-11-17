namespace AcBlog.Data.Documents
{
    public record Document
    {
        public string Tag { get; init; } = string.Empty;

        public string Raw { get; init; } = string.Empty;
    }
}

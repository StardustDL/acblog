namespace AcBlog.Data.Models
{
    public record User : RHasId<string>
    {
        public string Name { get; init; } = string.Empty;

        public string NickName { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;
    }
}

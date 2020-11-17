namespace AcBlog.Data.Models.Actions
{
    public record UserQueryRequest : QueryRequest
    {
        public string NickName { get; init; } = string.Empty;
    }
}

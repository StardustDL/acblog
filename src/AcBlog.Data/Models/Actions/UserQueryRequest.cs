namespace AcBlog.Data.Models.Actions
{
    public class UserQueryRequest : QueryRequest
    {
        public string Nickname { get; set; } = string.Empty;
    }
}

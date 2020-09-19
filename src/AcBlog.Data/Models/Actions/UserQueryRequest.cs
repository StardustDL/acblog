namespace AcBlog.Data.Models.Actions
{
    public class UserQueryRequest : QueryRequest
    {
        public string NickName { get; set; } = string.Empty;
    }
}

namespace AcBlog.Data.Models.Actions
{
    public class UserQueryRequest
    {
        public string Id { get; set; } = string.Empty;

        public string Nickname { get; set; } = string.Empty;

        public Pagination? Pagination { get; set; } = null;
    }
}

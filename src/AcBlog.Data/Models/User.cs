namespace AcBlog.Data.Models
{
    public class User : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string NickName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}

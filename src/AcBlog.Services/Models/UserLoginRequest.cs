namespace AcBlog.Services.Models
{
    public class UserLoginRequest
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}

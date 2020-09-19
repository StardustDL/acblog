namespace AcBlog.Services.Models
{
    public class UserChangePasswordRequest
    {
        public UserLoginRequest LoginRequest { get; set; } = new UserLoginRequest();

        public string NewPassword { get; set; } = string.Empty;
    }
}

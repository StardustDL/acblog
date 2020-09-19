using AcBlog.Data.Models;

namespace AcBlog.Data.Repositories.SqlServer.Models
{
    public class RawUser : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public static RawUser From(User value)
        {
            return new RawUser
            {
                Id = value.Id,
                Name = value.NickName,
                Email = value.Email,
            };
        }

        public static User To(RawUser value)
        {
            return new User
            {
                Id = value.Id,
                NickName = value.Name,
                Email = value.Email,
            };
        }
    }
}

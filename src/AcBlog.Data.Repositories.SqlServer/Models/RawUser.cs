using AcBlog.Data.Models;

namespace AcBlog.Data.Repositories.SqlServer.Models
{
    public class RawUser : IHasId<string>
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string NickName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public static RawUser From(User value)
        {
            return new RawUser
            {
                Id = value.Id,
                NickName = value.NickName,
                Name = value.Name,
                Email = value.Email,
            };
        }

        public static User To(RawUser value)
        {
            return new User
            {
                Id = value.Id,
                Name = value.Name,
                NickName = value.NickName,
                Email = value.Email,
            };
        }
    }
}

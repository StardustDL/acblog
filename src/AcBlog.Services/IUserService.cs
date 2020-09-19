using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Services.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Services
{
    public interface IUserService : IUserRepository
    {
        IBlogService BlogService { get; }

        Task<string> Login(UserLoginRequest request, CancellationToken cancellationToken = default);

        Task<bool> ChangePassword(UserChangePasswordRequest request, CancellationToken cancellationToken = default);
    }
}

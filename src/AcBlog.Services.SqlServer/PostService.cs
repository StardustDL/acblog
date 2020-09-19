using AcBlog.Data.Documents;
using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.SqlServer;
using AcBlog.Data.Repositories.SqlServer.Models;
using AcBlog.Services.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Services.SqlServer
{
    internal class PostService : RecordRepoBasedService<Post, string, PostQueryRequest, IPostRepository>, IPostService
    {
        public PostService(IBlogService blog, BlogDataContext dataContext) : base(blog, new PostDBRepository(dataContext))
        {
            Protector = new DocumentProtector();
        }

        public IProtector<Document> Protector { get; }

        public Task<CategoryTree> GetCategories(CancellationToken cancellationToken = default) => Repository.GetCategories(cancellationToken);

        public Task<KeywordCollection> GetKeywords(CancellationToken cancellationToken = default) => Repository.GetKeywords(cancellationToken);
    }

    internal class UserService : RecordRepoBasedService<User, string, UserQueryRequest, IUserRepository>, IUserService
    {
        public UserService(IBlogService blog, BlogDataContext dataContext) : base(blog, new UserDBRepository(dataContext))
        {
        }

        public Task<bool> ChangePassword(UserChangePasswordRequest request, CancellationToken cancellationToken = default) => throw new System.NotImplementedException();
        public Task<string> Login(UserLoginRequest request, CancellationToken cancellationToken = default) => throw new System.NotImplementedException();
    }
}

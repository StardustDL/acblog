using AcBlog.SDK;
using System.Threading.Tasks;
using Test.Data.Repositories;

namespace Test.SDK
{
    public abstract class SDKTest : RepositoriyTest
    {
        protected async Task UserService(IUserService service)
        {
            await UserRepository(service);
        }

        protected async Task PostService(IPostService service)
        {
            await PostRepository(service);
        }

        protected async Task BlogService(IBlogService service)
        {
            await UserService(service.UserService);
            await PostService(service.ArticleService);
            await PostService(service.SlidesService);
        }
    }
}

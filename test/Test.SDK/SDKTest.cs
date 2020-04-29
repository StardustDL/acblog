using AcBlog.SDK;
using System.Threading.Tasks;
using Test.Data.Providers;

namespace Test.SDK
{
    public abstract class SDKTest : ProviderTest
    {
        protected async Task UserService(IUserService service)
        {
            await UserProvider(service);
        }

        protected async Task PostService(IPostService service)
        {
            await PostProvider(service);
        }

        protected async Task BlogService(IBlogService service)
        {
            await UserService(service.UserService);
            await PostService(service.PostService);
        }
    }
}

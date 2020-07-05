using AcBlog.Sdk;
using System.Threading.Tasks;
using Test.Data.Repositories;

namespace Test.SDK
{
    public abstract class SdkTest : RepositoriyTest
    {
        protected async Task PostService(IPostService service)
        {
            await PostRepository(service);
        }

        protected async Task BlogService(IBlogService service)
        {
            await PostService(service.PostService);
        }
    }
}

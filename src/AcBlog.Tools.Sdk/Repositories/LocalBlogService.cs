using AcBlog.Sdk;
using Microsoft.Extensions.FileProviders;

namespace AcBlog.Tools.Sdk.Repositories
{
    public class LocalBlogService : IBlogService
    {
        public LocalBlogService(IFileProvider fileProvider)
        {
            FileProvider = fileProvider;
            PostService = new PostService(this, "posts", FileProvider);
        }

        public IFileProvider FileProvider { get; }

        public IPostService PostService { get; private set; }
    }
}

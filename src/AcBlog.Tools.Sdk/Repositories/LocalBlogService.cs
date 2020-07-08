using AcBlog.Sdk;
using StardustDL.Extensions.FileProviders;
using System.IO;

namespace AcBlog.Tools.Sdk.Repositories
{
    public class LocalBlogService : IBlogService
    {
        public LocalBlogService(string absPath, IFileProvider fileProvider)
        {
            FileProvider = fileProvider;
            PostService = new PostService(this, Path.Join(absPath, "posts"), "posts", FileProvider);
        }

        public IFileProvider FileProvider { get; }

        public IPostService PostService { get; private set; }
    }
}

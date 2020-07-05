using StardustDL.Extensions.FileProviders;
using System;
using System.Text;

namespace AcBlog.Sdk.FileSystem
{
    public class FileSystemBlogService : IBlogService
    {
        public FileSystemBlogService(IFileProvider fileProvider)
        {
            FileProvider = fileProvider;
            PostService = new PostService(this, "posts", FileProvider);
        }

        public IFileProvider FileProvider { get; }

        public IPostService PostService { get; private set; }
    }
}

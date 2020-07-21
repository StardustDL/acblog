using AcBlog.Data.Models;
using StardustDL.Extensions.FileProviders;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk.FileSystem
{
    public class FileSystemBlogService : IBlogService
    {
        public FileSystemBlogService(IFileProvider fileProvider)
        {
            FileProvider = fileProvider;
            PostService = new PostService(this, "posts", FileProvider);
            PageService = new PageService(this, "pages", FileProvider);
            LayoutService = new LayoutService(this, "layouts", FileProvider);
        }

        public IFileProvider FileProvider { get; }

        public IPostService PostService { get; }

        public IPageService PageService { get; }

        public ILayoutService LayoutService { get; }

        public async Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default)
        {
            using var fs = await (await FileProvider.GetFileInfo("blog.json").ConfigureAwait(false)).CreateReadStream().ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<BlogOptions>(fs, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}

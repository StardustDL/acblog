using AcBlog.Data.Models;
using AcBlog.Sdk;
using AcBlog.Tools.Sdk.Models;
using StardustDL.Extensions.FileProviders;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Repositories
{
    public class LocalBlogService : IBlogService
    {
        public LocalBlogService(string rootPath)
        {
            RootPath = rootPath;
            PostService = new PostService(this, Path.Join(rootPath, "posts"));
            PageService = new PageService(this, Path.Join(rootPath, "pages"));
            LayoutService = new LayoutService(this, Path.Join(rootPath, "layouts"));
        }

        public string RootPath { get; }

        public IPostService PostService { get; }

        public IPageService PageService { get; }

        public ILayoutService LayoutService { get; }

        public ICommentService CommentService => throw new System.NotImplementedException();

        public IStatisticService StatisticService => throw new System.NotImplementedException();

        public async Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default)
        {
            string path = Path.Join(RootPath, Workspace.BlogOptionPath);
            if (File.Exists(path))
            {
                using var st = File.OpenRead(path);
                return await JsonSerializer.DeserializeAsync<BlogOptions>(st).ConfigureAwait(false);
            }
            else
            {
                return new BlogOptions();
            }
        }
    }
}

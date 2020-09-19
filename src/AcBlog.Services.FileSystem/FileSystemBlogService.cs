using AcBlog.Data.Models;
using AcBlog.Data.Repositories;
using StardustDL.Extensions.FileProviders;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Services.FileSystem
{
    public class FileSystemBlogService : IBlogService
    {
        public FileSystemBlogService(IFileProvider fileProvider)
        {
            FileProvider = fileProvider;
            PostService = new PostService(this, "posts", FileProvider);
            PageService = new PageService(this, "pages", FileProvider);
            LayoutService = new LayoutService(this, "layouts", FileProvider);
            FileService = new FileService(this, "files", FileProvider);
        }

        public IFileProvider FileProvider { get; }

        public IPostService PostService { get; }

        public IPageService PageService { get; }

        public ILayoutService LayoutService { get; }

        public IFileService FileService { get; }

        public ICommentService CommentService => throw new NotImplementedException();

        public IStatisticService StatisticService => throw new NotImplementedException();

        public IUserService UserService => throw new NotImplementedException();

        public RepositoryAccessContext Context => new RepositoryAccessContext();

        public async Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default)
        {
            await using var fs = await (await FileProvider.GetFileInfo("blog.json").ConfigureAwait(false)).CreateReadStream().ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<BlogOptions>(fs, cancellationToken: cancellationToken)
                .ConfigureAwait(false) ?? throw new NullReferenceException("Options is null");
        }

        public Task<bool> SetOptions(BlogOptions options, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}

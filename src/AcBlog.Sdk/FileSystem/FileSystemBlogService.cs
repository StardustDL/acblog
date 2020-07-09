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
        }

        public IFileProvider FileProvider { get; }

        public IPostService PostService { get; private set; }

        public async Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default)
        {
            using var fs = await(await FileProvider.GetFileInfo("blog.json").ConfigureAwait(false)).CreateReadStream().ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<BlogOptions>(fs, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}

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
        public LocalBlogService(string absPath, IFileProvider fileProvider)
        {
            FileProvider = fileProvider;
            PostService = new PostService(this, Path.Join(absPath, "posts"), "posts", FileProvider);
        }

        public IFileProvider FileProvider { get; }

        public IPostService PostService { get; private set; }

        public async Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default)
        {
            var file = await FileProvider.GetFileInfo(Workspace.BlogOptionPath);

            if(await file.Exists())
            {
                using var st = await file.CreateReadStream();
                return await JsonSerializer.DeserializeAsync<BlogOptions>(st);
            }
            else
            {
                return new BlogOptions();
            }
        }
    }
}

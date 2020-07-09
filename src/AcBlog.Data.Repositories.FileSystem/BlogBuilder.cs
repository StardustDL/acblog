using AcBlog.Data.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem
{
    public class BlogBuilder
    {
        public BlogBuilder(BlogOptions options, string rootPath)
        {
            RootPath = rootPath;

            Options = options;

            FsBuilder = new FSBuilder(rootPath);
        }

        public string RootPath { get; }

        public BlogOptions Options { get; }

        public FSBuilder FsBuilder { get; }

        public async Task Build()
        {
            FsBuilder.EnsureDirectoryEmpty();
            using var st = FsBuilder.GetFileRewriteStream($"blog.json");
            await JsonSerializer.SerializeAsync(st, Options).ConfigureAwait(false);
        }
    }
}

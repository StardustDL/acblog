using AcBlog.Data.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Builders
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

        public async Task BuildPosts(IList<Post> data)
        {
            var builder = new PostRepositoryBuilder(Path.Join(RootPath, "posts"));
            await builder.Build(data);
        }

        public async Task BuildPages(IList<Page> data)
        {
            var builder = new PageRepositoryBuilder(Path.Join(RootPath, "pages"));
            await builder.Build(data);
        }

        public async Task BuildLayouts(IList<Layout> data)
        {
            var builder = new LayoutRepositoryBuilder(Path.Join(RootPath, "layouts"));
            await builder.Build(data);
        }

        public async Task BuildFiles(IList<Models.File> data)
        {
            var builder = new FileRepositoryBuilder(Path.Join(RootPath, "files"));
            await builder.Build(data);
        }
    }
}

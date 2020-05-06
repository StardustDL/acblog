using AcBlog.Data.Models;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization.NamingConventions;

namespace AcBlog.Tool.StaticGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string dist = Path.Join(Environment.CurrentDirectory, "dist");
            if (Directory.Exists(dist))
                Directory.Delete(dist, true);

            Directory.CreateDirectory(dist);
            {
                string postDist = Path.Join(dist, "articles");
                Directory.CreateDirectory(postDist);

                var loader = new PostsLoader(new DirectoryInfo(Path.Join(Environment.CurrentDirectory, "articles")),
                    new PostProtector());

                var ls = await loader.LoadAll();
                Array.ForEach(ls, x => x.Type = PostType.Article);

                await PostRepositoryBuilder.Build(ls, postDist, 10);
            }
            {
                string postDist = Path.Join(dist, "slides");
                Directory.CreateDirectory(postDist);

                var loader = new PostsLoader(new DirectoryInfo(Path.Join(Environment.CurrentDirectory, "slides")),
                    new PostProtector());

                var ls = await loader.LoadAll();
                Array.ForEach(ls, x => x.Type = PostType.Slides);

                await PostRepositoryBuilder.Build(ls, postDist, 10);
            }
        }
    }
}

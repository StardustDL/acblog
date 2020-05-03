using AcBlog.Data.Models;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                string postDist = Path.Join(dist, "posts");
                Directory.CreateDirectory(postDist);

                DirectoryInfo di = new DirectoryInfo(Path.Join(Environment.CurrentDirectory, "posts"));
                List<Post> posts = new List<Post>();

                foreach (var fi in di.GetFiles("*.md", SearchOption.AllDirectories))
                {
                    Post post = new Post();
                    post.Title = Path.GetFileNameWithoutExtension(fi.Name);
                    post.CreationTime = fi.CreationTimeUtc;
                    post.ModificationTime = fi.LastWriteTimeUtc;
                    post.Category = fi.DirectoryName
                        .Replace(di.FullName, "")
                        .Replace("\\", "/")
                        .Split("/")
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrEmpty(x))
                        .ToArray();
                    post.Id = Guid.NewGuid().ToString();
                    {
                        using var fs = fi.OpenRead();
                        using var reader = new StreamReader(fs);
                        post.Content = await reader.ReadToEndAsync();
                    }
                    posts.Add(post);
                    Console.WriteLine($"Loaded {fi.FullName}");
                }
                await PostRepositoryBuilder.Build(posts, postDist, 10);
            }
        }
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AcBlog.Tool.StaticGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string dist = Path.Join(Environment.CurrentDirectory, "dist");
            if (Directory.Exists(dist))
                Directory.Delete(dist);
            Directory.CreateDirectory(dist);
            {
                string postDist = Path.Join(dist, "posts");
                Directory.CreateDirectory(postDist);
                List<Post> posts = new List<Post>();
                foreach (var f in Directory.GetFiles(Environment.CurrentDirectory, "posts/*.md", SearchOption.AllDirectories))
                {
                    Post post = new Post();
                    FileInfo fi = new FileInfo(f);
                    post.Title = Path.GetFileNameWithoutExtension(fi.Name);
                    post.CreationTime = fi.CreationTimeUtc;
                    post.ModificationTime = fi.LastWriteTimeUtc;
                    post.Id = Guid.NewGuid().ToString();
                    {
                        using var fs = fi.OpenRead();
                        using var reader = new StreamReader(fs);
                        post.Content = await reader.ReadToEndAsync();
                    }
                    posts.Add(post);
                }
                await PostRepositoryBuilder.Build(posts, postDist, 10);
            }
        }
    }
}

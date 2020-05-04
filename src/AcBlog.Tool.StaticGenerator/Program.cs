using AcBlog.Data.Models;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AcBlog.Tool.StaticGenerator
{
    class PostMetadata
    {
        public string title { get; set; }

        public DateTime? date { get; set; }

        public IList<string> keywords { get; set; }

        public IList<string> category { get; set; }

        public string password { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            PostProtector = new PostProtector();

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
                    Post post = null;
                    try
                    {
                        post = await LoadPost(di, fi);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                        return;
                    }
                    Console.WriteLine($"Loaded {fi.FullName}");
                    posts.Add(post);
                }
                await PostRepositoryBuilder.Build(posts, postDist, 10);
            }
        }
        
        static PostProtector PostProtector { get; set; }

        static async Task<Post> LoadPost(DirectoryInfo root, FileInfo file)
        {
            Post post = new Post();
            post.Title = Path.GetFileNameWithoutExtension(file.Name);
            post.CreationTime = file.CreationTimeUtc;
            post.ModificationTime = file.LastWriteTimeUtc;
            post.Category = file.DirectoryName
                .Replace(root.FullName, "")
                .Replace("\\", "/")
                .Split("/")
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();
            post.Id = Guid.NewGuid().ToString();
            string rawText = "";
            {
                using var fs = file.OpenRead();
                using var reader = new StreamReader(fs);
                rawText = await reader.ReadToEndAsync();
            }
            var lines = rawText.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n");
            int contentBg = 0;
            PostMetadata metadata = null;
            if (lines.Length > 0)
            {
                if (lines[0].Length >= 3 && lines[0].All(c => c == '-'))
                {
                    int l = 1, r = 1;
                    for (; r < lines.Length; r++)
                    {
                        if (lines[r] == lines[0])
                            break;
                    }

                    var deserializer = new DeserializerBuilder().Build();
                    var yaml = string.Join('\n', lines[l..r]);
                    contentBg = r;
                    try
                    {
                        metadata = deserializer.Deserialize<PostMetadata>(yaml);
                        if (metadata.title != null)
                            post.Title = metadata.title;
                        if (metadata.keywords != null)
                            post.Keywords = metadata.keywords.ToArray();
                        if (metadata.date != null)
                            post.CreationTime = metadata.date.Value.ToUniversalTime();
                        if (metadata.category != null)
                            post.Category = metadata.category.ToArray();
                    }
                    catch
                    {
                        Console.WriteLine("Failed to parse post metadata.");
                    }
                }
            }
            post.Content = string.Join('\n', lines[contentBg..]);
            if(metadata != null && !string.IsNullOrEmpty(metadata.password))
            {
                post = await PostProtector.Protect(post, new ProtectionKey { Password = metadata.password });
            }
            return post;
        }
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Protections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace AcBlog.Tools.StaticGenerator
{
    public class PostsLoader
    {
        public PostsLoader(DirectoryInfo root, IProtector<Post> protector)
        {
            Root = root;
            Protector = protector;
        }

        public DirectoryInfo Root { get; }

        public IProtector<Post> Protector { get; }

        public async Task<Post> Load(FileInfo file)
        {
            Post post = new Post();
            post.Title = Path.GetFileNameWithoutExtension(file.Name);
            post.CreationTime = file.CreationTimeUtc;
            post.ModificationTime = file.LastWriteTimeUtc;
            post.Category = file.DirectoryName
                .Replace(Root.FullName, "")
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
            if (contentBg + 1 < lines.Length)
            {
                post.Content = string.Join('\n', lines[(contentBg + 1)..]);
            }
            else post.Content = "";
            if (metadata != null && !string.IsNullOrEmpty(metadata.password))
            {
                post = await Protector.Protect(post, new ProtectionKey { Password = metadata.password });
            }
            return post;
        }

        public async Task<Post[]> LoadAll()
        {
            List<Post> posts = new List<Post>();

            if (Root.Exists)
            {
                foreach (var fi in Root.GetFiles("*.md", SearchOption.AllDirectories))
                {
                    Post post = null;
                    try
                    {
                        post = await Load(fi);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw ex;
                    }
                    Console.WriteLine($"Loaded {fi.FullName}");
                    posts.Add(post);
                }
            }

            return posts.ToArray();
        }
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories.FileSystem;
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
        public PostsLoader(DirectoryInfo root)
        {
            Root = root;
        }

        public DirectoryInfo Root { get; }

        public async Task<PostBuildData> Load(FileInfo file)
        {
            PostBuildData result = new PostBuildData(new Post());
            var post = result.Raw;
            post.Title = Path.GetFileNameWithoutExtension(file.Name);
            post.CreationTime = file.CreationTimeUtc;
            post.ModificationTime = file.LastWriteTimeUtc;
            result.Category = file.DirectoryName
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
                            result.Keywords = metadata.keywords.ToArray();
                        if (metadata.date != null)
                            post.CreationTime = metadata.date.Value.ToUniversalTime();
                        if (metadata.category != null)
                            result.Category = metadata.category.ToArray();
                        if (metadata.type != null)
                        {
                            post.Type = metadata.type switch
                            {
                                "slides" => PostType.Slides,
                                "note" => PostType.Note,
                                _ => PostType.Article
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to parse post metadata.");
                        Console.WriteLine(ex);
                    }
                }
            }
            if (contentBg + 1 < lines.Length)
            {
                post.Content = new Document
                {
                    Raw = string.Join('\n', lines[(contentBg + 1)..])
                };
            }
            else post.Content = new Document();
            if (metadata != null && !string.IsNullOrEmpty(metadata.password))
            {
                result.Key = new ProtectionKey
                {
                    Password = metadata.password
                };
            }
            return result;
        }

        public async Task<PostBuildData[]> LoadAll()
        {
            List<PostBuildData> posts = new List<PostBuildData>();

            if (Root.Exists)
            {
                foreach (var fi in Root.GetFiles("*.md", SearchOption.AllDirectories))
                {
                    try
                    {
                        var post = await Load(fi);
                        Console.WriteLine($"Loaded {fi.FullName}");
                        posts.Add(post);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw ex;
                    }

                }
            }

            return posts.ToArray();
        }
    }
}

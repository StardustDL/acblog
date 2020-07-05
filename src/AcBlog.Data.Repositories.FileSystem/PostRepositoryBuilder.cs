using AcBlog.Data.Models;
using AcBlog.Data.Repositories.FileSystem.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Linq;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using System.Security.Cryptography;
using AcBlog.Data.Documents;
using Microsoft.Extensions.FileProviders;

namespace AcBlog.Data.Repositories.FileSystem
{
    public class PostRepositoryBuilder
    {
        public PostRepositoryBuilder(IList<Post> data, string rootPath)
        {
            RootPath = rootPath;

            Data = data;

            FsBuilder = new FSBuilder(rootPath);
        }

        public PagingConfig PagingConfig { get; set; } = new PagingConfig();

        public IProtector<Document>? Protector { get; set; }

        public string RootPath { get; }

        protected IList<Post> Data { get; set; }

        FSBuilder FsBuilder { get; }

        async Task BuildIndexType()
        {
            {
                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, "articles"));

                await paging.Build(Data.Where(
                    x => x.Type == PostType.Article).ToList().Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }

            {
                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, "slides"));

                await paging.Build(Data.Where(
                    x => x.Type == PostType.Slides).ToList().Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }

            {
                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, "notes"));

                await paging.Build(Data.Where(
                    x => x.Type == PostType.Note).ToList().Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }
        }

        async Task BuildIndexKeyword()
        {
            FsBuilder.EnsureDirectoryEmpty("keywords");

            Dictionary<string, List<Post>> dict = new Dictionary<string, List<Post>>();
            foreach (var v in Data)
            {
                List<string> keyids = new List<string>();
                foreach (var k in v.Keywords.Items)
                {
                    if (!dict.ContainsKey(k))
                    {
                        var list = new List<Post>
                        {
                            v
                        };
                        dict.Add(k, list);
                    }
                    else
                    {
                        dict[k].Add(v);
                    }
                }
            }

            foreach (var v in dict)
            {
                string subdir = Path.Join("keywords", NameUtility.Encode(v.Key));

                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, subdir));

                await paging.Build(v.Value.Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }

            // TODO Add All keyword api
        }

        private class CategoryNode
        {
            public CategoryNode(Category category)
            {
                Category = category;
            }

            public Category Category { get; set; }

            public IList<Post> Data { get; } = new List<Post>();

            public Dictionary<string, CategoryNode> Children { get; } = new Dictionary<string, CategoryNode>();
        }

        async Task BuildIndexCategory()
        {
            FsBuilder.EnsureDirectoryEmpty("categories");

            CategoryNode root = new CategoryNode(new Category());
            foreach (var v in Data)
            {
                CategoryNode node = root;
                foreach (var k in v.Category.Items)
                {
                    if (!node.Children.ContainsKey(k))
                    {
                        Category c = new Category
                        {
                            Items = node.Category.Items.Concat(new[] { k }),
                        };
                        var tn = new CategoryNode(c);
                        node.Children.Add(k, tn);
                        node = tn;
                    }
                    else
                    {
                        node = node.Children[k];
                    }
                    node.Data.Add(v);
                }
            }

            Queue<CategoryNode> q = new Queue<CategoryNode>();
            foreach (var v in root.Children.Values)
                q.Enqueue(v);

            while (q.Count > 0)
            {
                var node = q.Dequeue();

                string subdir = Path.Join("categories", Path.Combine(node.Category.Items.Select(NameUtility.Encode).ToArray()));

                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, subdir));

                await paging.Build(node.Data.Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);

                foreach (var v in node.Children.Values)
                    q.Enqueue(v);
            }

            // TODO Add All category api
        }

        public async Task Build()
        {
            FsBuilder.EnsureDirectoryEmpty();

            Data = (from x in Data orderby x.CreationTime descending select x).ToArray();

            await BuildIndexType().ConfigureAwait(false);

            await BuildIndexKeyword().ConfigureAwait(false);

            await BuildIndexCategory().ConfigureAwait(false);

            foreach (var v in Data)
            {
                Post post = v;
                using var st = FsBuilder.GetFileRewriteStream($"{NameUtility.Encode(post.Id)}.json");
                await JsonSerializer.SerializeAsync(st, post);
            }

            {
                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, "pages"));

                await paging.Build(Data.Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }
        }
    }
}

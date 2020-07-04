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
        public PostRepositoryBuilder(IList<PostBuildData> data, string rootPath)
        {
            RootPath = rootPath;

            Data = data;

            FsBuilder = new FSBuilder(rootPath);
        }

        public PagingConfig PagingConfig { get; set; } = new PagingConfig();

        public IProtector<Document>? Protector { get; set; }

        public string RootPath { get; }

        protected IList<PostBuildData> Data { get; set; }

        FSBuilder FsBuilder { get; }

        async Task BuildIndexType()
        {
            {
                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, "articles"));

                await paging.Build(Data.Where(
                    x => x.Raw.Type == PostType.Article).ToList().Select(x => x.Raw.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }

            {
                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, "slides"));

                await paging.Build(Data.Where(
                    x => x.Raw.Type == PostType.Slides).ToList().Select(x => x.Raw.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }

            {
                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, "notes"));

                await paging.Build(Data.Where(
                    x => x.Raw.Type == PostType.Note).ToList().Select(x => x.Raw.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }
        }

        async Task BuildIndexKeyword()
        {
            FsBuilder.EnsureDirectoryEmpty("keywords");

            Dictionary<string, List<PostBuildData>> dict = new Dictionary<string, List<PostBuildData>>();
            foreach (var v in Data)
            {
                List<string> keyids = new List<string>();
                foreach (var k in v.Raw.Keywords)
                {
                    if (!dict.ContainsKey(k))
                    {
                        var list = new List<PostBuildData>
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
                string subdir = Path.Join("keywords", v.Key);

                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, subdir));

                await paging.Build(v.Value.Select(x => x.Raw.Id).ToArray(),
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

            public IList<PostBuildData> Data { get; } = new List<PostBuildData>();

            public Dictionary<string, CategoryNode> Children { get; } = new Dictionary<string, CategoryNode>();
        }

        async Task BuildIndexCategory()
        {
            FsBuilder.EnsureDirectoryEmpty("categories");

            CategoryNode root = new CategoryNode(new Category());
            foreach (var v in Data)
            {
                CategoryNode node = root;
                foreach (var k in v.Raw.Category)
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

            while(q.Count > 0)
            {
                var node = q.Dequeue();

                string subdir = Path.Join("categories", Path.Combine(node.Category.ToArray()));

                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, subdir));

                await paging.Build(node.Data.Select(x => x.Raw.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);

                foreach (var v in node.Children.Values)
                    q.Enqueue(v);
            }

            // TODO Add All category api
        }

        public async Task Build()
        {
            Data = (from x in Data orderby x.Raw.CreationTime descending select x).ToArray();

            await BuildIndexType();

            await BuildIndexKeyword();

            await BuildIndexCategory();

            foreach (var v in Data)
            {
                Post post = v.Raw;
                if (v.Key != null && Protector != null)
                    post.Content = await Protector.Protect(post.Content, v.Key);

                await JsonSerializer.SerializeAsync(FsBuilder.GetFileRewriteStream($"{post.Id}.json"), post);
            }

            {
                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, "pages"));

                await paging.Build(Data.Select(x => x.Raw.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }
        }
    }
}

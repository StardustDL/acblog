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
using AcBlog.Data.Extensions;

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

            KeywordCollector collector = KeywordCollector.BuildFromPosts(Data);

            foreach (var v in collector.Nodes)
            {
                string subdir = Path.Join("keywords", NameUtility.Encode(v.Keyword.Items.First()));

                PagingProvider<string> paging = new PagingProvider<string>(Path.Join(RootPath, subdir));

                await paging.Build(v.Data.Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }

            // TODO Add All keyword api
        }

        async Task BuildIndexCategory()
        {
            FsBuilder.EnsureDirectoryEmpty("categories");

            CategoryTree tree = CategoryTree.BuildFromPosts(Data);

            Queue<CategoryTree.Node> q = new Queue<CategoryTree.Node>();
            foreach (var v in tree.Root.Children.Values)
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

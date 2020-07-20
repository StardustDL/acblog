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
        }

        public PagingConfig PagingConfig { get; set; } = new PagingConfig();

        public IProtector<Document>? Protector { get; set; }

        public string RootPath { get; }

        protected IList<Post> Data { get; set; }

        async Task BuildIndexType()
        {
            {
                PagingProvider<string> paging = new PagingProvider<string>(Paths.GetArticleRoot(RootPath));

                await paging.Build(Data.Where(
                    x => x.Type == PostType.Article).ToList().Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }

            {
                PagingProvider<string> paging = new PagingProvider<string>(Paths.GetSlidesRoot(RootPath));

                await paging.Build(Data.Where(
                    x => x.Type == PostType.Slides).ToList().Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }

            {
                PagingProvider<string> paging = new PagingProvider<string>(Paths.GetNoteRoot(RootPath));

                await paging.Build(Data.Where(
                    x => x.Type == PostType.Note).ToList().Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }
        }

        async Task BuildIndexKeyword()
        {
            FSStaticBuilder.EnsureDirectoryEmpty(Paths.GetKeywordRoot(RootPath));

            var (collection, map) = KeywordCollectionBuilder.BuildFromPosts(Data);

            foreach (var v in collection.Items)
            {
                PagingProvider<string> paging = new PagingProvider<string>(Paths.GetKeywordRoot(RootPath, v));

                await paging.Build(map[v.OneName()].Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }

            using var st = FSStaticBuilder.GetFileRewriteStream(Paths.GetKeywordMetadata(RootPath));
            await JsonSerializer.SerializeAsync(st, collection).ConfigureAwait(false);
        }

        async Task BuildIndexCategory()
        {
            FSStaticBuilder.EnsureDirectoryEmpty(Paths.GetCategoryRoot(RootPath));

            var (tree, map) = CategoryTreeBuilder.BuildFromPosts(Data);

            Queue<CategoryTree.Node> q = new Queue<CategoryTree.Node>();
            foreach (var v in tree.Root.Children.Values)
                q.Enqueue(v);

            while (q.Count > 0)
            {
                var node = q.Dequeue();

                PagingProvider<string> paging = new PagingProvider<string>(Paths.GetCategoryRoot(RootPath, node.Category));

                await paging.Build(map[node].Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);

                foreach (var v in node.Children.Values)
                    q.Enqueue(v);
            }

            using var st = FSStaticBuilder.GetFileRewriteStream(Paths.GetCategoryMetadata(RootPath));
            await JsonSerializer.SerializeAsync(st, tree).ConfigureAwait(false);
        }

        public async Task Build()
        {
            FSStaticBuilder.EnsureDirectoryEmpty(RootPath);

            Data = (from x in Data orderby x.CreationTime descending select x).ToArray();

            await BuildIndexType().ConfigureAwait(false);

            await BuildIndexKeyword().ConfigureAwait(false);

            await BuildIndexCategory().ConfigureAwait(false);

            foreach (var v in Data)
            {
                Post post = v;
                using var st = FSStaticBuilder.GetFileRewriteStream(Paths.GetFileById(RootPath, post.Id));
                await JsonSerializer.SerializeAsync(st, post).ConfigureAwait(false);
            }

            {
                PagingProvider<string> paging = new PagingProvider<string>(Paths.GetPaginationRoot(RootPath));

                await paging.Build(Data.Select(x => x.Id).ToArray(),
                    PagingConfig).ConfigureAwait(false);
            }
        }
    }
}

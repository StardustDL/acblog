using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class PostReaderBase : ReaderBase<Post, string, PostQueryRequest>, IPostRepository
    {
        protected PostReaderBase(string rootPath) : base(rootPath)
        {
        }

        public override async Task<QueryResponse<string>> Query(PostQueryRequest query, CancellationToken cancellationToken = default)
        {
            query.Pagination ??= new Pagination();

            PagingPath? paging = null;

            if (query.Type != null)
            {
                switch (query.Type)
                {
                    case PostType.Article:
                        paging = new PagingPath(Path.Join(RootPath, "articles"));
                        break;
                    case PostType.Slides:
                        paging = new PagingPath(Path.Join(RootPath, "slides"));
                        break;
                    case PostType.Note:
                        paging = new PagingPath(Path.Join(RootPath, "notes"));
                        break;
                }
            }
            else if (!string.IsNullOrWhiteSpace(query.CategoryId))
            {
                var catRoot = Path.Join(RootPath, "categories");
                paging = new PagingPath(Path.Join(catRoot, query.CategoryId));
            }
            else if (!string.IsNullOrWhiteSpace(query.KeywordId))
            {
                var catRoot = Path.Join(RootPath, "keywords");
                paging = new PagingPath(Path.Join(catRoot, query.KeywordId));
            }

            paging ??= new PagingPath(Path.Join(RootPath, "pages"));

            await EnsurePagingConfig(paging);
            paging.FillPagination(query.Pagination);

            var res = new QueryResponse<string>(await GetPagingResult(paging, query.Pagination, cancellationToken), query.Pagination);
            return res;
        }
    }

    public class PostLocalReader : PostReaderBase
    {
        public PostLocalReader(string rootPath) : base(rootPath)
        {
        }

        public override Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(File.Exists(GetPath(id)));
        }

        protected override Task<Stream> GetFileReadStream(string path, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Stream>(File.Open(path, FileMode.Open, FileAccess.Read));
        }
    }

    public class PostRemoteReader : PostReaderBase
    {
        public PostRemoteReader(string rootPath, HttpClient client) : base(rootPath)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public override async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            var rep = await Client.GetAsync(GetPath(id), cancellationToken);
            return rep.IsSuccessStatusCode;
        }

        protected override async Task<Stream> GetFileReadStream(string path, CancellationToken cancellationToken = default)
        {
            var rep = await Client.GetAsync(path, cancellationToken);
            rep.EnsureSuccessStatusCode();
            return await rep.Content.ReadAsStreamAsync();
        }
    }
}

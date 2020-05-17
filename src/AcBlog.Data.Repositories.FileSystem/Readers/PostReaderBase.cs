using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class PostReaderBase : ReaderBase<Post, string>, IPostRepository
    {
        protected PostReaderBase(string rootPath) : base(rootPath)
        {
        }

        private string PathNormalize(string path) => path.Replace("\\", "/");

        protected override string GetPath(string id) => PathNormalize(Path.Join(RootPath, $"{id}.json"));

        public override async Task<IEnumerable<string>> All(CancellationToken cancellationToken = default)
        {
            List<string> result = new List<string>();
            PostQueryRequest pq = new PostQueryRequest();
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var req = await Query(pq, cancellationToken);
                result.AddRange(req.Results);
                if (!req.CurrentPage.HasNextPage)
                    break;
                pq.Pagination = req.CurrentPage.NextPage();
            }
            return result;
        }

        public virtual async Task<QueryResponse<string>> Query(PostQueryRequest query, CancellationToken cancellationToken = default)
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
}

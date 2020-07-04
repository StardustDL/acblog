using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using Microsoft.Extensions.FileProviders;
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
    public abstract class PostFSReader : RecordFSReader<Post, string, PostQueryRequest>, IPostRepository
    {
        protected PostFSReader(string rootPath, IFileProvider fileProvider) : base(rootPath, fileProvider)
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

            await EnsurePagingConfig(paging)
                .ConfigureAwait(false);
            paging.FillPagination(query.Pagination);

            var res = new QueryResponse<string>(
                await GetPagingResult(paging, query.Pagination, cancellationToken).ConfigureAwait(false), 
                query.Pagination);
            return res;
        }
    }
}

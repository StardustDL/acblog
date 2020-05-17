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
    public abstract class UserReaderBase : ReaderBase<User, string>, IUserRepository
    {
        protected UserReaderBase(string rootPath) : base(rootPath)
        {
        }

        protected override string GetPath(string id) => Path.Join(RootPath, $"{id}.json").Replace("\\", "/");

        public override async Task<IEnumerable<string>> All(CancellationToken cancellationToken = default)
        {
            List<string> result = new List<string>();
            UserQueryRequest pq = new UserQueryRequest();
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

        public virtual async Task<QueryResponse<string>> Query(UserQueryRequest query, CancellationToken cancellationToken = default)
        {
            query.Pagination ??= new Pagination();

            PagingPath? paging = null;

            paging ??= new PagingPath(Path.Join(RootPath, "pages"));

            await EnsurePagingConfig(paging, cancellationToken);
            paging.FillPagination(query.Pagination);

            var res = new QueryResponse<string>(await GetPagingResult(paging, query.Pagination, cancellationToken), query.Pagination);
            return res;
        }
    }
}

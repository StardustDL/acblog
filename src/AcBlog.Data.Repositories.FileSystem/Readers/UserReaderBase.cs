using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class UserReaderBase : ReaderBase<User, string>, IUserRepository
    {
        protected UserReaderBase(string rootPath) : base(rootPath)
        {
        }

        protected override string GetPath(string id) => Path.Join(RootPath, $"{id}.json").Replace("\\", "/");

        public override async Task<User?> Get(string id)
        {
            var res = await base.Get(id);
            if (res != null)
                res.Id = id;
            return res;
        }

        public override async Task<IEnumerable<string>> All()
        {
            List<string> result = new List<string>();
            UserQueryRequest pq = new UserQueryRequest();
            while (true)
            {
                var req = await Query(pq);
                result.AddRange(req.Results);
                if (!req.CurrentPage.HasNextPage)
                    break;
                pq.Pagination = req.CurrentPage.NextPage();
            }
            return result;
        }

        public virtual async Task<QueryResponse<string>> Query(UserQueryRequest query)
        {
            query.Pagination ??= new Pagination();

            PagingPath paging = new PagingPath(RootPath);

            await EnsurePagingConfig(paging);
            paging.FillPagination(query.Pagination);

            var res = new QueryResponse<string>(await GetPagingResult(paging, query.Pagination), query.Pagination);
            return res;
        }
    }
}

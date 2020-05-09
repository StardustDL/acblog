using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class KeywordReaderBase : ReaderBase<Keyword, string>, IKeywordRepository
    {
        protected KeywordReaderBase(string rootPath) : base(rootPath)
        {
        }

        protected override string GetPath(string id) => Path.Join(RootPath, $"{id}.json").Replace("\\", "/");

        public override async Task<Keyword?> Get(string id)
        {
            var res = await base.Get(id);
            if (res != null)
                res.Id = id;
            return res;
        }

        public override async Task<IEnumerable<string>> All()
        {
            List<string> result = new List<string>();
            KeywordQueryRequest pq = new KeywordQueryRequest();
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

        public virtual async Task<QueryResponse<string>> Query(KeywordQueryRequest query)
        {
            query.Pagination ??= new Pagination();

            PagingPath? paging = null;

            paging ??= new PagingPath(Path.Join(RootPath, "pages"));

            await EnsurePagingConfig(paging);
            paging.FillPagination(query.Pagination);

            var res = new QueryResponse<string>(await GetPagingResult(paging, query.Pagination), query.Pagination);
            return res;
        }
    }
}

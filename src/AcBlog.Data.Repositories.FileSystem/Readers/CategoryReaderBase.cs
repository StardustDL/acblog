using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class CategoryReaderBase : ReaderBase<Category, string>, ICategoryRepository
    {
        protected CategoryReaderBase(string rootPath) : base(rootPath)
        {
        }

        protected override string GetPath(string id) => Path.Join(RootPath, $"{id}.json").Replace("\\", "/");

        public override async Task<IEnumerable<string>> All()
        {
            List<string> result = new List<string>();
            CategoryQueryRequest pq = new CategoryQueryRequest();
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

        public virtual async Task<QueryResponse<string>> Query(CategoryQueryRequest query)
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

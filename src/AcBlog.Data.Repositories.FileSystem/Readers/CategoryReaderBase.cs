using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class CategoryReaderBase : ReaderBase<Category, string, CategoryQueryRequest>, ICategoryRepository
    {
        protected CategoryReaderBase(string rootPath) : base(rootPath)
        {
        }

        protected override string GetPath(string id) => Path.Join(RootPath, $"{id}.json").Replace("\\", "/");

        public override async Task<IEnumerable<string>> All(CancellationToken cancellationToken = default)
        {
            List<string> result = new List<string>();
            CategoryQueryRequest pq = new CategoryQueryRequest();
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

        public override async Task<QueryResponse<string>> Query(CategoryQueryRequest query, CancellationToken cancellationToken = default)
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

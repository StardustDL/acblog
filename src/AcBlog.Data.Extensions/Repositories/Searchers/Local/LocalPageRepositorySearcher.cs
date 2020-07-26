using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers.Local
{
    public class LocalPageRepositorySearcher : IPageRepositorySearcher
    {
        public LocalPageRepositorySearcher(IPageRepository repository) => Repository = repository;

        public IPageRepository Repository { get; }

        public async Task<QueryResponse<string>> Search(PageQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = (await Repository.GetAllItems(cancellationToken)).IgnoreNull();

            if (string.IsNullOrEmpty(query.Route))
                qr.Where(x => x.Route.StartsWith(query.Route));

            return qr.AsQueryResponse<Page, string>(query);
        }
    }
}

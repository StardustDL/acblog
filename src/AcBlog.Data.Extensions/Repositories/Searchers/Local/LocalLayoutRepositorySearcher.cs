using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers.Local
{
    public class LocalLayoutRepositorySearcher : ILayoutRepositorySearcher
    {
        public LocalLayoutRepositorySearcher(ILayoutRepository repository) => Repository = repository;

        public ILayoutRepository Repository { get; }

        public async Task<QueryResponse<string>> Search(LayoutQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = (await Repository.GetAllItems(cancellationToken)).IgnoreNull();

            return qr.AsQueryResponse<Layout, string>(query);
        }
    }
}

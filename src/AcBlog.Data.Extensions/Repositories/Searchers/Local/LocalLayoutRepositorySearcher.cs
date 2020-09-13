using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Linq;
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
            var qr = Repository.GetAllItems(cancellationToken).IgnoreNull();

            return (await qr.ToArrayAsync(cancellationToken)).AsQueryResponse<Layout, string>(query);
        }
    }
}

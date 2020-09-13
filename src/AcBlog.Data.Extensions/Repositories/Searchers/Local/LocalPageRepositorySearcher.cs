using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
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
            var qr = Repository.GetAllItems(cancellationToken).IgnoreNull();

            if (string.IsNullOrEmpty(query.Route))
                qr = qr.Where(x => x.Route.StartsWith(query.Route));
            if (!string.IsNullOrWhiteSpace(query.Term))
            {
                qr = qr.Where(x =>
                    x.Title.ToString().Contains(query.Term) ||
                    x.Content.ToString().Contains(query.Term)
                );
            }

            return (await qr.ToArrayAsync(cancellationToken)).AsQueryResponse<Page, string>(query);
        }
    }
}

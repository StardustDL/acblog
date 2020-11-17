using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers.Local
{
    public class LocalPageRepositorySearcher : IPageRepositorySearcher
    {
        public IAsyncEnumerable<string> Search(IPageRepository repository, PageQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = repository.GetAllItems(cancellationToken).IgnoreNull();
            if (string.IsNullOrEmpty(query.Route))
                qr = qr.Where(x => x.Route.StartsWith(query.Route));
            if (!string.IsNullOrWhiteSpace(query.Term))
            {
                qr = qr.Where(x =>
                    x.Title.ToString().Contains(query.Term) ||
                    x.Content.ToString().Contains(query.Term)
                );
            }
            return qr.Select(item => item.Id).IgnoreNull().Paging(query.Pagination);
        }
    }
}

using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers.Local
{
    public class LocalStatisticRepositorySearcher : IStatisticRepositorySearcher
    {
        public IAsyncEnumerable<string> Search(IStatisticRepository repository, StatisticQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = repository.GetAllItems(cancellationToken).IgnoreNull();

            if (!string.IsNullOrWhiteSpace(query.Category))
                qr = qr.Where(x => x.Category == query.Category);
            if (!string.IsNullOrWhiteSpace(query.Uri))
                qr = qr.Where(x => x.Uri == query.Uri);

            return qr.Select(item => item.Id).Paging(query.Pagination);
        }
    }
}

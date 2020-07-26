using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers.Local
{
    public class LocalStatisticRepositorySearcher : IStatisticRepositorySearcher
    {
        public LocalStatisticRepositorySearcher(IStatisticRepository repository) => Repository = repository;

        public IStatisticRepository Repository { get; }

        public async Task<QueryResponse<string>> Search(StatisticQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = (await Repository.GetAllItems(cancellationToken)).IgnoreNull();

            if (!string.IsNullOrWhiteSpace(query.Category))
                qr = qr.Where(x => x.Category == query.Category);
            if (!string.IsNullOrWhiteSpace(query.Uri))
                qr = qr.Where(x => x.Uri == query.Uri);

            return qr.AsQueryResponse<Statistic, string>(query);
        }
    }
}

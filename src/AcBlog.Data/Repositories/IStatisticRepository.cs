using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories
{
    public interface IStatisticRepository : IRecordRepository<Statistic, string, StatisticQueryRequest>
    {
        Task<long> Count(StatisticQueryRequest query, CancellationToken cancellationToken = default);
    }
}

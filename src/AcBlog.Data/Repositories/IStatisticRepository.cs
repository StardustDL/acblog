using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories
{
    public interface IStatisticRepository : IRecordRepository<Statistic, string, StatisticQueryRequest>
    {
    }
}

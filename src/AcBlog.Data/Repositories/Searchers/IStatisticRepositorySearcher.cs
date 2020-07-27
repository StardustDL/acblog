using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories.Searchers
{
    public interface IStatisticRepositorySearcher : IRecordRepositorySearcher<Statistic, string, StatisticQueryRequest, IStatisticRepository>
    {

    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk
{
    public static class StatisticServiceExtensions
    {
        public static IStatisticService AsService(this IStatisticRepository repository, IBlogService blogService)
        {
            return new RepoBasedService(blogService, repository);
        }

        class RepoBasedService : RecordRepoBaseService<Statistic, string, StatisticQueryRequest, IStatisticRepository>, IStatisticService
        {
            public RepoBasedService(IBlogService blogService, IStatisticRepository repository) : base(blogService, repository)
            {
            }

            public Task<long> Count(StatisticQueryRequest query, CancellationToken cancellationToken = default) => Repository.Count(query, cancellationToken);
        }
    }
}

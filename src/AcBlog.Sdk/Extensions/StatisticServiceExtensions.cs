using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk.Extensions
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

        public static string GetStatisticUri(this Post value)
        {
            return $"posts/{value.Id}";
        }

        public static string GetStatisticUri(this Page value)
        {
            return $"pages/{value.Id}";
        }
    }
}

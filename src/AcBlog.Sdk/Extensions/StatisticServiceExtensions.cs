using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;

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
        }
    }
}

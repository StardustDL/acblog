using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.SqlServer;
using AcBlog.Data.Repositories.SqlServer.Models;

namespace AcBlog.Services.SqlServer
{
    internal class StatisticService : RecordRepoBasedService<Statistic, string, StatisticQueryRequest, IStatisticRepository>, IStatisticService
    {
        public StatisticService(IBlogService blog, BlogDataContext dataContext) : base(blog, new StatisticDBRepository(dataContext))
        {
        }
    }
}

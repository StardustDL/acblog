using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Services;
using System.Net.Http;

namespace AcBlog.Sdk.Api
{
    internal class StatisticService : BaseRecordApiService<Statistic, StatisticQueryRequest, IStatisticRepositorySearcher>, IStatisticService
    {
        public StatisticService(IBlogService blog, HttpClient httpClient) : base(blog, httpClient)
        {
        }

        protected override string PrepUrl => "/Statistics";
    }
}

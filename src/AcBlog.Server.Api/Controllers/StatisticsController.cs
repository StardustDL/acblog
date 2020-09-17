using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcBlog.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatisticsController : RecordControllerBase<Statistic, string, IStatisticRepository, StatisticQueryRequest>
    {
        public StatisticsController(IBlogService service) : base(service.StatisticService)
        {

        }
    }
}

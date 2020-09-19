using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcBlog.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatisticsController : RecordControllerBase<Statistic, string, IStatisticRepository, StatisticQueryRequest>
    {
        public StatisticsController(IBlogService service) : base(service.StatisticService)
        {

        }

        [AllowAnonymous]
        public override Task<ActionResult<string>> Create([FromBody] Statistic value) => base.Create(value);
    }
}

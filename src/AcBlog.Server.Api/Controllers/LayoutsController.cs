using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcBlog.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LayoutsController : RecordControllerBase<Layout, ILayoutRepository, LayoutQueryRequest>
    {
        public LayoutsController(IBlogService service) : base(service.LayoutService)
        {

        }
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcBlog.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PagesController : RecordControllerBase<Page, string, IPageRepository, PageQueryRequest>
    {
        public PagesController(IBlogService service) : base(service.PageService)
        {

        }
    }
}

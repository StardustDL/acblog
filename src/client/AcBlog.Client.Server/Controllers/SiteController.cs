using AcBlog.Sdk;
using AcBlog.Services;
using AcBlog.Services.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AcBlog.Client.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private IBlogService BlogService { get; }

        public IConfiguration Configuration { get; }

        private string BaseAddress { get; }

        public SiteController(IBlogService blogService, IConfiguration configuration)
        {
            BlogService = blogService;
            Configuration = configuration;
            BaseAddress = Configuration.GetBaseAddress().TrimEnd('/');
        }

        [HttpGet("sitemap")]
        [HttpGet("sitemap.xml")]
        public async Task<ActionResult> GetSitemap()
        {
            var result = await BlogService.QuerySitemap(BaseAddress);
            return Content(result.Result, "text/xml");
        }

        [HttpGet("atom")]
        [HttpGet("atom.xml")]
        public async Task<ActionResult> GetAtomFeed()
        {
            var result = await BlogService.QueryAtomFeed(BaseAddress);
            return Content(result.Result, "text/xml");
        }
    }
}
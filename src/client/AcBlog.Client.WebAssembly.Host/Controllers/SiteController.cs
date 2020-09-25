using AcBlog.Sdk;
using AcBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AcBlog.Client.WebAssembly.Host.Controllers
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
    }
}
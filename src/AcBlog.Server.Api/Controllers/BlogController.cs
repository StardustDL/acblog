using AcBlog.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcBlog.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        [HttpGet]
        public Task<BlogOptions> Options()
        {
            // TODO: update options
            return Task.FromResult(new BlogOptions());
        }
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Services;
using AcBlog.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcBlog.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        public BlogController(IBlogService blogService)
        {
            BlogService = blogService;
        }

        public IBlogService BlogService { get; }


        [HttpGet("options")]
        public async Task<BlogOptions> GetOptions()
        {
            return await BlogService.GetOptions();
        }

        [HttpPost("query")]
        public async Task<QueryResponse<string>> Query([FromBody] BlogQueryRequest query)
        {
            return await BlogService.Query(query);
        }

        [HttpPost("options")]
        [Authorize]
        public async Task<bool> SetOptions([FromBody] BlogOptions options)
        {
            return await BlogService.SetOptions(options);
        }
    }
}

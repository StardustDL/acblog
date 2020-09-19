using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcBlog.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : RecordControllerBase<Post, IPostService, PostQueryRequest>
    {
        public PostsController(IBlogService service) : base(service.PostService)
        {

        }

        [HttpGet("categories")]
        public async Task<ActionResult<CategoryTree>> GetCategories()
        {
            return await Service.GetCategories();
        }

        [HttpGet("keywords")]
        public async Task<ActionResult<KeywordCollection>> GetKeywords()
        {
            return await Service.GetKeywords();
        }
    }
}

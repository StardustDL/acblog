using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcBlog.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : RecordControllerBase<Post, string, IPostRepository, PostQueryRequest>
    {
        public PostsController(IPostRepository repository) : base(repository)
        {

        }

        [HttpGet("categories")]
        public async Task<ActionResult<CategoryTree>> GetCategories()
        {
            return await Repository.GetCategories();
        }

        [HttpGet("keywords")]
        public async Task<ActionResult<KeywordCollection>> GetKeywords()
        {
            return await Repository.GetKeywords();
        }
    }
}

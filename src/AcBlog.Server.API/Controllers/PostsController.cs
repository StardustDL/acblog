using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcBlog.Server.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : RecordControllerBase<Post, string, IPostRepository, PostQueryRequest>
    {
        public PostsController(IPostRepository repository) : base(repository)
        {

        }
    }
}

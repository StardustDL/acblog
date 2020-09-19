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
    public class CommentsController : RecordControllerBase<Comment, ICommentRepository, CommentQueryRequest>
    {
        public CommentsController(IBlogService service) : base(service.CommentService)
        {

        }

        [AllowAnonymous]
        public override Task<ActionResult<string>> Create([FromBody] Comment value) => base.Create(value);
    }
}

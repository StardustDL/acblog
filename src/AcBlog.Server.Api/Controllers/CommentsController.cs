using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcBlog.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentsController : RecordControllerBase<Comment, string, ICommentRepository, CommentQueryRequest>
    {
        public CommentsController(IBlogService service) : base(service.CommentService)
        {

        }
    }
}

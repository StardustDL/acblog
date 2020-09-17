using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.SqlServer;
using AcBlog.Data.Repositories.SqlServer.Models;

namespace AcBlog.Services.SqlServer
{
    internal class CommentService : RecordRepoBasedService<Comment, string, CommentQueryRequest, ICommentRepository>, ICommentService
    {
        public CommentService(IBlogService blog, BlogDataContext dataContext) : base(blog, new CommentDBRepository(dataContext))
        {
        }
    }
}

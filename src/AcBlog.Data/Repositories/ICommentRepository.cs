using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories
{
    public interface ICommentRepository : IRecordRepository<Comment, string, CommentQueryRequest>
    {
    }
}

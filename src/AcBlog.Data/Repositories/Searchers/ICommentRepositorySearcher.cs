using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories.Searchers
{
    public interface ICommentRepositorySearcher : IRecordRepositorySearcher<Comment, string, CommentQueryRequest, ICommentRepository>
    {

    }
}

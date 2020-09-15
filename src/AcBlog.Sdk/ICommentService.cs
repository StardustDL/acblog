using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;

namespace AcBlog.Sdk
{
    public interface ICommentService : ICommentRepository
    {
        IBlogService BlogService { get; }
    }
}

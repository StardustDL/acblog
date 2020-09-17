using AcBlog.Data.Repositories;

namespace AcBlog.Services
{
    public interface ICommentService : ICommentRepository
    {
        IBlogService BlogService { get; }
    }
}

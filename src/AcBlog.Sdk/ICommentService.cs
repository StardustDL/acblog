using AcBlog.Data.Repositories;

namespace AcBlog.Sdk
{
    public interface ICommentService : ICommentRepository
    {
        IBlogService BlogService { get; }
    }
}

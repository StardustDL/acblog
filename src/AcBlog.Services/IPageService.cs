using AcBlog.Data.Repositories;

namespace AcBlog.Services
{
    public interface IPageService : IPageRepository
    {
        IBlogService BlogService { get; }
    }
}

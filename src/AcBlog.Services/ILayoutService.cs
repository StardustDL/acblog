using AcBlog.Data.Repositories;

namespace AcBlog.Services
{
    public interface ILayoutService : ILayoutRepository
    {
        IBlogService BlogService { get; }
    }
}

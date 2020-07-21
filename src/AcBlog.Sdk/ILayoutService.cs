using AcBlog.Data.Repositories;

namespace AcBlog.Sdk
{
    public interface ILayoutService : ILayoutRepository
    {
        IBlogService BlogService { get; }
    }
}

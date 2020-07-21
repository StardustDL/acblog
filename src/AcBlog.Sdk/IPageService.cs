using AcBlog.Data.Repositories;

namespace AcBlog.Sdk
{
    public interface IPageService : IPageRepository
    {
        IBlogService BlogService { get; }
    }
}

using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;

namespace AcBlog.Sdk
{
    public interface IPageService : IPageRepository
    {
        IBlogService BlogService { get; }
    }
}

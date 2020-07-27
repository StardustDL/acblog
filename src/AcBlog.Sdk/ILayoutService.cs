using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;

namespace AcBlog.Sdk
{
    public interface ILayoutService : ILayoutRepository
    {
        IBlogService BlogService { get; }

        ILayoutRepositorySearcher Searcher { get; }
    }
}

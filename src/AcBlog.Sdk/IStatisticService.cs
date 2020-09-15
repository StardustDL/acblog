using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;

namespace AcBlog.Sdk
{
    public interface IStatisticService : IStatisticRepository
    {
        IBlogService BlogService { get; }
    }
}

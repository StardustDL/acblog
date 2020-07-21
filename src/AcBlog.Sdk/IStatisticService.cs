using AcBlog.Data.Repositories;

namespace AcBlog.Sdk
{
    public interface IStatisticService : IStatisticRepository
    {
        IBlogService BlogService { get; }
    }
}

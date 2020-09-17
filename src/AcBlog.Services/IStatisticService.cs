using AcBlog.Data.Repositories;

namespace AcBlog.Services
{
    public interface IStatisticService : IStatisticRepository
    {
        IBlogService BlogService { get; }
    }
}

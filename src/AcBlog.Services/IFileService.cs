using AcBlog.Data.Repositories;

namespace AcBlog.Services
{
    public interface IFileService : IFileRepository
    {
        IBlogService BlogService { get; }
    }
}

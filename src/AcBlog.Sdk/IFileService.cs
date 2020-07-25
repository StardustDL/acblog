using AcBlog.Data.Repositories;

namespace AcBlog.Sdk
{
    public interface IFileService : IFileRepository
    {
        IBlogService BlogService { get; }
    }
}

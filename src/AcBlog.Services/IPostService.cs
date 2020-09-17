using AcBlog.Data.Documents;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;

namespace AcBlog.Services
{
    public interface IPostService : IPostRepository
    {
        IBlogService BlogService { get; }

        IProtector<Document> Protector { get; }
    }
}

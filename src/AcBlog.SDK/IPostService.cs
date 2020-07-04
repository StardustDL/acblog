using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;

namespace AcBlog.SDK
{
    public interface IPostService : IPostRepository
    {
        IBlogService BlogService { get; }

        IProtector<Document> Protector { get; }
    }
}

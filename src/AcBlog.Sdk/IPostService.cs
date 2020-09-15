using AcBlog.Data.Documents;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;

namespace AcBlog.Sdk
{
    public interface IPostService : IPostRepository
    {
        IBlogService BlogService { get; }

        IProtector<Document> Protector { get; }
    }
}

using AcBlog.Data.Repositories;

namespace AcBlog.SDK
{
    public interface IKeywordService : IKeywordRepository
    {
        IBlogService Blog { get; }
    }
}

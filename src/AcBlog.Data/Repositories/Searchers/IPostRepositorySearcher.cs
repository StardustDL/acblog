using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories.Searchers
{
    public interface IPostRepositorySearcher : IRecordRepositorySearcher<Post, string, PostQueryRequest, IPostRepository>
    {

    }
}

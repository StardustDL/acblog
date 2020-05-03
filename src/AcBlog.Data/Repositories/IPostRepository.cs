using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories
{
    public interface IPostRepository : IRecordRepository<Post, string>
    {
        Task<QueryResponse<string>> Query(PostQueryRequest query);
    }
}

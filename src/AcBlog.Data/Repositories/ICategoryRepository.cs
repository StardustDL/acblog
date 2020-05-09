using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories
{
    public interface ICategoryRepository : IRecordRepository<Category, string>
    {
        Task<QueryResponse<string>> Query(CategoryQueryRequest query);
    }
}

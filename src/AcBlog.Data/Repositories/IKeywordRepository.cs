using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories
{
    public interface IKeywordRepository : IRecordRepository<Keyword, string>
    {
        Task<QueryResponse<string>> Query(KeywordQueryRequest query, CancellationToken cancellationToken = default);
    }
}

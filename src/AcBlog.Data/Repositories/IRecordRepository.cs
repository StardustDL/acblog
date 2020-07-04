using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories
{

    public interface IRecordRepository<T, TId, TQuery> : IRepository where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
    {
        Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default);

        Task<IEnumerable<TId>> All(CancellationToken cancellationToken = default);

        Task<bool> Exists(TId id, CancellationToken cancellationToken = default);

        Task<T?> Get(TId id, CancellationToken cancellationToken = default);

        Task<bool> Delete(TId id, CancellationToken cancellationToken = default);

        Task<bool> Update(T value, CancellationToken cancellationToken = default);

        Task<TId?> Create(T value, CancellationToken cancellationToken = default);

        Task<QueryResponse<TId>> Query(TQuery query, CancellationToken cancellationToken = default);
    }
}

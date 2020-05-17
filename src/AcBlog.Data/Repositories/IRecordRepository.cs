using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories
{
    public interface IRecordRepository<T, TId> : IRepository where TId : class where T : class
    {
        Task<bool> CanRead(CancellationToken cancellationToken = default);

        Task<bool> CanWrite(CancellationToken cancellationToken = default);

        Task<IEnumerable<string>> All(CancellationToken cancellationToken = default);

        Task<bool> Exists(TId id, CancellationToken cancellationToken = default);

        Task<T?> Get(TId id, CancellationToken cancellationToken = default);

        Task<bool> Delete(TId id, CancellationToken cancellationToken = default);

        Task<bool> Update(T value, CancellationToken cancellationToken = default);

        Task<TId?> Create(T value, CancellationToken cancellationToken = default);
    }
}

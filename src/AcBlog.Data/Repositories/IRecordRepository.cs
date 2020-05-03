using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories
{
    public interface IRecordRepository<T, TId> : IRepository where TId : class where T : class
    {
        Task<bool> CanRead();

        Task<bool> CanWrite();

        Task<IEnumerable<string>> All();

        Task<bool> Exists(TId id);

        Task<T?> Get(TId id);

        Task<bool> Delete(TId id);

        Task<bool> Update(T value);

        Task<TId?> Create(T value);
    }
}

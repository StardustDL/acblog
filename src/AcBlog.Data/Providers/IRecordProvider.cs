using System.Threading.Tasks;

namespace AcBlog.Data.Providers
{
    public interface IRecordProvider<T, TId> : IProvider
    {
        bool IsReadable { get; }

        bool IsWritable { get; }

        Task<bool> Exists(TId id);

        Task<T> Get(TId id);

        Task<bool> Delete(TId id);

        Task<bool> Update(T value);

        Task<TId> Create(T value);
    }
}

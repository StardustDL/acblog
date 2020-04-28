namespace AcBlog.Data.Providers
{
    public interface IRecordProvider<T, TId>
    {
        bool IsReadable { get; }

        bool IsWritable { get; }

        bool Exists(TId id);

        T Get(TId id);

        bool Delete(TId id);

        bool Update(T value);

        TId Create(T value);
    }
}

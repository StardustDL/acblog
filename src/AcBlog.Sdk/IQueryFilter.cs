using AcBlog.Data.Models.Actions;
using System.Threading.Tasks;

namespace AcBlog.Sdk
{
    public interface IQueryFilter<TBase>
    {
        TBase BaseService { get; }

        Task<QueryResponse<string>> Filter(Pagination? pagination);
    }

    public interface IQueryFilter<TBase, T>
    {
        TBase BaseService { get; }

        Task<QueryResponse<string>> Filter(T arg, Pagination? pagination);
    }
}

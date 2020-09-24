using AcBlog.Data;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Services
{
    public interface IQueryFilter<TService, TResult>
    {
        TService BaseService { get; }

        Task<PagingData<TResult>> Filter(Pagination? pagination, QueryTimeOrder order = QueryTimeOrder.None);
    }

    public interface IQueryFilter<TService, TResult, T>
    {
        TService BaseService { get; }

        Task<PagingData<TResult>> Filter(T arg, Pagination? pagination, QueryTimeOrder order = QueryTimeOrder.None);
    }
}

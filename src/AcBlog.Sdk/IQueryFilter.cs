using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Sdk
{
    public interface IQueryFilter<TService, TResult>
    {
        TService BaseService { get; }

        IAsyncEnumerable<TResult> Filter(Pagination? pagination);
    }

    public interface IQueryFilter<TService, TResult, T>
    {
        TService BaseService { get; }

        IAsyncEnumerable<TResult> Filter(T arg, Pagination? pagination);
    }
}

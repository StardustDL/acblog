using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Sdk.Filters
{
    public abstract class BaseQueryFilter<TService, TResult> : IQueryFilter<TService, TResult>
    {
        protected BaseQueryFilter(TService baseService)
        {
            BaseService = baseService;
        }

        public TService BaseService { get; protected set; }

        public abstract IAsyncEnumerable<TResult> Filter(Pagination? pagination = null);
    }

    public abstract class BaseQueryFilter<TService, TResult, T> : IQueryFilter<TService, TResult, T>
    {
        protected BaseQueryFilter(TService baseService)
        {
            BaseService = baseService;
        }

        public TService BaseService { get; protected set; }

        public abstract IAsyncEnumerable<TResult> Filter(T arg, Pagination? pagination = null);
    }
}

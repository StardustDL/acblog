using AcBlog.Data;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Services.Filters
{
    public abstract class BaseQueryFilter<TService, TResult> : IQueryFilter<TService, TResult>
    {
        protected BaseQueryFilter(TService baseService)
        {
            BaseService = baseService;
        }

        public TService BaseService { get; protected set; }

        public abstract Task<PagingData<TResult>> Filter(Pagination? pagination = null, QueryTimeOrder order = QueryTimeOrder.None);
    }

    public abstract class BaseQueryFilter<TService, TResult, T> : IQueryFilter<TService, TResult, T>
    {
        protected BaseQueryFilter(TService baseService)
        {
            BaseService = baseService;
        }

        public TService BaseService { get; protected set; }

        public abstract Task<PagingData<TResult>> Filter(T arg, Pagination? pagination = null, QueryTimeOrder order = QueryTimeOrder.None);
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Threading.Tasks;

namespace AcBlog.SDK.Filters
{
    public abstract class BaseQueryFilter<TBase> : IQueryFilter<TBase>
    {
        protected BaseQueryFilter(TBase baseService)
        {
            BaseService = baseService;
        }

        public TBase BaseService { get; protected set; }

        public abstract Task<QueryResponse<string>> Filter(Pagination? pagination);
    }

    public abstract class BaseQueryFilter<TBase, T> : IQueryFilter<TBase, T>
    {
        protected BaseQueryFilter(TBase baseService)
        {
            BaseService = baseService;
        }

        public TBase BaseService { get; protected set; }

        public abstract Task<QueryResponse<string>> Filter(T arg, Pagination? pagination);
    }
}

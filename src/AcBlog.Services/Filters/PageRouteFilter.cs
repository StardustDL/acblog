using AcBlog.Data;
using AcBlog.Data.Extensions;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Services.Filters
{
    public class PageRouteFilter : BaseQueryFilter<IPageService, string, string?>
    {
        public PageRouteFilter(IPageService baseService) : base(baseService)
        {
        }

        public override Task<PagingData<string>> Filter(string? arg, Pagination? pagination = null, QueryTimeOrder order = QueryTimeOrder.None)
        {
            string route = (arg ?? string.Empty);
            return BaseService.QueryPaging(new PageQueryRequest
            {
                Route = route,
                Pagination = pagination,
                Order = order,
            });
        }
    }
}

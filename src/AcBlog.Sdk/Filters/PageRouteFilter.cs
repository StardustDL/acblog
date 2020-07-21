using AcBlog.Data.Models.Actions;
using System.Threading.Tasks;

namespace AcBlog.Sdk.Filters
{
    public class PageRouteFilter : BaseQueryFilter<IPageService, string>
    {
        public PageRouteFilter(IPageService baseService) : base(baseService)
        {
        }

        public override Task<QueryResponse<string>> Filter(string? arg, Pagination? pagination)
        {
            string route = (arg ?? string.Empty).Trim('/');
            return BaseService.Query(new PageQueryRequest
            {
                Route = route,
                Pagination = pagination
            });
        }
    }
}

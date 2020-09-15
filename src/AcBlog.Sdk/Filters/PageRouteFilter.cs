using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Sdk.Filters
{
    public class PageRouteFilter : BaseQueryFilter<IPageService, string, string?>
    {
        public PageRouteFilter(IPageService baseService) : base(baseService)
        {
        }

        public override IAsyncEnumerable<string> Filter(string? arg, Pagination? pagination = null)
        {
            string route = (arg ?? string.Empty);
            return BaseService.Query(new PageQueryRequest
            {
                Route = route,
                Pagination = pagination
            });
        }
    }
}

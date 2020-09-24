using AcBlog.Data;
using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Services.Filters
{
    public class PostCategoryFilter : BaseQueryFilter<IPostService, string, Category?>
    {
        public PostCategoryFilter(IPostService baseService) : base(baseService)
        {
        }

        public override Task<PagingData<string>> Filter(Category? arg, Pagination? pagination = null, QueryTimeOrder order = QueryTimeOrder.None)
        {
            return BaseService.QueryPaging(new PostQueryRequest
            {
                Category = arg,
                Pagination = pagination,
                Order = order,
            });
        }
    }
}

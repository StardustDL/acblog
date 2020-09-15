using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Sdk.Filters
{
    public class PostCategoryFilter : BaseQueryFilter<IPostService, string, Category?>
    {
        public PostCategoryFilter(IPostService baseService) : base(baseService)
        {
        }

        public override IAsyncEnumerable<string> Filter(Category? arg, Pagination? pagination = null)
        {
            return BaseService.Query(new PostQueryRequest
            {
                Category = arg,
                Pagination = pagination
            });
        }
    }
}

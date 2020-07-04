using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Threading.Tasks;

namespace AcBlog.Sdk.Filters
{
    public class PostCategoryFilter : BaseQueryFilter<IPostService, Category?>
    {
        public PostCategoryFilter(IPostService baseService) : base(baseService)
        {
        }

        public override Task<QueryResponse<string>> Filter(Category? arg, Pagination? pagination)
        {
            return BaseService.Query(new PostQueryRequest
            {
                Category = arg,
                Pagination = pagination
            });
        }
    }
}

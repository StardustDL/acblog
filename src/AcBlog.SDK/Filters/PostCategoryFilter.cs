using AcBlog.Data.Models.Actions;
using System.Threading.Tasks;

namespace AcBlog.SDK.Filters
{
    public class PostCategoryFilter : BaseQueryFilter<IPostService, string>
    {
        public PostCategoryFilter(IPostService baseService) : base(baseService)
        {
        }

        public override Task<QueryResponse<string>> Filter(string arg, Pagination? pagination)
        {
            return BaseService.Query(new PostQueryRequest
            {
                CategoryId = arg,
                Pagination = pagination
            });
        }
    }
}

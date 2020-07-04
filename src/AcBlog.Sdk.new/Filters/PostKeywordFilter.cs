using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Threading.Tasks;

namespace AcBlog.Sdk.Filters
{
    public class PostKeywordFilter : BaseQueryFilter<IPostService, Keyword?>
    {
        public PostKeywordFilter(IPostService baseService) : base(baseService)
        {
        }

        public override Task<QueryResponse<string>> Filter(Keyword? arg, Pagination? pagination)
        {
            return BaseService.Query(new PostQueryRequest
            {
                Keywords = arg,
                Pagination = pagination
            });
        }
    }
}

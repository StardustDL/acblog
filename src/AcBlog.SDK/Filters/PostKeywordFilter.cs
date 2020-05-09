using AcBlog.Data.Models.Actions;
using System.Threading.Tasks;

namespace AcBlog.SDK.Filters
{
    public class PostKeywordFilter : BaseQueryFilter<IPostService, string>
    {
        public PostKeywordFilter(IPostService baseService) : base(baseService)
        {
        }

        public override Task<QueryResponse<string>> Filter(string arg, Pagination? pagination)
        {
            return BaseService.Query(new PostQueryRequest
            {
                KeywordId = arg,
                Pagination = pagination
            });
        }
    }
}

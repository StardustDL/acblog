using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Sdk.Filters
{
    public class PostKeywordFilter : BaseQueryFilter<IPostService, string, Keyword?>
    {
        public PostKeywordFilter(IPostService baseService) : base(baseService)
        {
        }

        public override IAsyncEnumerable<string> Filter(Keyword? arg, Pagination? pagination = null)
        {
            return BaseService.Query(new PostQueryRequest
            {
                Keywords = arg,
                Pagination = pagination
            });
        }
    }
}

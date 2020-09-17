using AcBlog.Data;
using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Services.Filters
{
    public class PostKeywordFilter : BaseQueryFilter<IPostService, string, Keyword?>
    {
        public PostKeywordFilter(IPostService baseService) : base(baseService)
        {
        }

        public override Task<PagingData<string>> Filter(Keyword? arg, Pagination? pagination = null)
        {
            return BaseService.QueryPaging(new PostQueryRequest
            {
                Keywords = arg,
                Pagination = pagination
            });
        }
    }
}

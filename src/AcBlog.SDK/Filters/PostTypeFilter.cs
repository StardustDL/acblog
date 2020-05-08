using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Threading.Tasks;

namespace AcBlog.SDK.Filters
{
    public class PostTypeFilter : BaseQueryFilter<IPostService, PostType?>
    {
        public PostTypeFilter(IPostService baseService) : base(baseService)
        {
        }

        public override Task<QueryResponse<string>> Filter(PostType? arg, Pagination? pagination)
        {
            return BaseService.Query(new PostQueryRequest
            {
                Type = arg,
                Paging = pagination
            });
        }
    }

    public class PostArticleFilter : PostTypeFilter, IQueryFilter<IPostService>
    {
        public PostArticleFilter(IPostService baseService) : base(baseService)
        {
        }

        public Task<QueryResponse<string>> Filter(Pagination? pagination) => base.Filter(PostType.Article, pagination);
    }

    public class PostSlidesFilter : PostTypeFilter, IQueryFilter<IPostService>
    {
        public PostSlidesFilter(IPostService baseService) : base(baseService)
        {
        }

        public Task<QueryResponse<string>> Filter(Pagination? pagination) => base.Filter(PostType.Slides, pagination);
    }
}

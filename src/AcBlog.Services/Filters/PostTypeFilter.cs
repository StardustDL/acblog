using AcBlog.Data;
using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Services.Filters
{
    public class PostTypeFilter : BaseQueryFilter<IPostService, string, PostType?>
    {
        public PostTypeFilter(IPostService baseService) : base(baseService)
        {
        }

        public override Task<PagingData<string>> Filter(PostType? arg = null, Pagination? pagination = null)
        {
            return BaseService.QueryPaging(new PostQueryRequest
            {
                Type = arg,
                Pagination = pagination
            });
        }
    }

    public class PostArticleFilter : PostTypeFilter, IQueryFilter<IPostService, string>
    {
        public PostArticleFilter(IPostService baseService) : base(baseService)
        {
        }

        public Task<PagingData<string>> Filter(Pagination? pagination = null) => base.Filter(PostType.Article, pagination);
    }

    public class PostSlidesFilter : PostTypeFilter, IQueryFilter<IPostService, string>
    {
        public PostSlidesFilter(IPostService baseService) : base(baseService)
        {
        }

        public Task<PagingData<string>> Filter(Pagination? pagination = null) => base.Filter(PostType.Slides, pagination);
    }

    public class PostNoteFilter : PostTypeFilter, IQueryFilter<IPostService, string>
    {
        public PostNoteFilter(IPostService baseService) : base(baseService)
        {
        }

        public Task<PagingData<string>> Filter(Pagination? pagination = null) => base.Filter(PostType.Note, pagination);
    }
}

using AcBlog.SDK.Filters;

namespace AcBlog.SDK
{
    public static class PostServiceExtensions
    {
        public static PostArticleFilter CreateArticleFilter(this IPostService service)
        {
            return new PostArticleFilter(service);
        }

        public static PostSlidesFilter CreateSlidesFilter(this IPostService service)
        {
            return new PostSlidesFilter(service);
        }

        public static PostNoteFilter CreateNoteFilter(this IPostService service)
        {
            return new PostNoteFilter(service);
        }

        public static PostKeywordFilter CreateKeywordFilter(this IPostService service)
        {
            return new PostKeywordFilter(service);
        }

        public static PostCategoryFilter CreateCategoryFilter(this IPostService service)
        {
            return new PostCategoryFilter(service);
        }
    }
}

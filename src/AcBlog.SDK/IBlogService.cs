using AcBlog.SDK.Filters;

namespace AcBlog.SDK
{
    public interface IBlogService
    {
        IUserService UserService { get; }

        IPostService PostService { get; }

        IKeywordService KeywordService { get; }

        ICategoryService CategoryService { get; }
    }
}

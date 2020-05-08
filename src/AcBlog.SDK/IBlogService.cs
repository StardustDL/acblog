using AcBlog.SDK.Filters;

namespace AcBlog.SDK
{
    public interface IBlogService
    {
        IUserService UserService { get; }

        IPostService PostService { get; }
    }
}

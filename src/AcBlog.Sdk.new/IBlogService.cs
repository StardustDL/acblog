using AcBlog.Sdk.Filters;

namespace AcBlog.Sdk
{
    public interface IBlogService
    {
        IPostService PostService { get; }
    }
}

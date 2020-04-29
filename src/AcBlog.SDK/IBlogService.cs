namespace AcBlog.SDK
{
    public interface IBlogService
    {
        IUserService UserService { get; }

        IPostService PostService { get; }
    }
}

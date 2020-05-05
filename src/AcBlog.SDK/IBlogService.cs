namespace AcBlog.SDK
{
    public interface IBlogService
    {
        IUserService UserService { get; }

        IPostService ArticleService { get; }

        IPostService SlidesService { get; }
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Services
{
    public interface IBlogService
    {
        IPostService PostService { get; }

        IPageService PageService { get; }

        ILayoutService LayoutService { get; }

        IUserService UserService { get; }

        ICommentService CommentService { get; }

        IStatisticService StatisticService { get; }

        IFileService FileService { get; }

        RepositoryAccessContext Context { get; }

        Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default);

        Task<bool> SetOptions(BlogOptions options, CancellationToken cancellationToken = default);
    }
}

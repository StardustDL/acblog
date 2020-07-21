using AcBlog.Data.Models;
using AcBlog.Sdk.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk
{
    public interface IBlogService
    {
        IPostService PostService { get; }

        IPageService PageService { get; }

        ILayoutService LayoutService { get; }

        ICommentService CommentService { get; }

        IStatisticService StatisticService { get; }

        Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default);
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Repositories.SqlServer.Models;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Services.SqlServer
{
    public class SqlServerBlogService : IBlogService
    {
        public SqlServerBlogService(BlogDataContext dataContext)
        {
            DataContext = dataContext;
            PostService = new PostService(this, DataContext);
            PageService = new PageService(this, DataContext);
            LayoutService = new LayoutService(this, DataContext);
            CommentService = new CommentService(this, DataContext);
            StatisticService = new StatisticService(this, DataContext);
        }

        public BlogDataContext DataContext { get; }

        public IPostService PostService { get; }

        public IPageService PageService { get; }

        public ILayoutService LayoutService { get; }

        public IFileService FileService => throw new NotImplementedException();

        public ICommentService CommentService { get; }

        public IStatisticService StatisticService { get; }

        public async Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

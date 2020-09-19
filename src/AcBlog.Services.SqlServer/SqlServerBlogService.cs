using AcBlog.Data.Models;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.SqlServer.Models;
using System;
using System.IO;
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
            UserService = new UserService(this, DataContext);
        }

        public BlogDataContext DataContext { get; }

        public IPostService PostService { get; }

        public IPageService PageService { get; }

        public ILayoutService LayoutService { get; }

        public IFileService FileService => throw new NotImplementedException();

        public ICommentService CommentService { get; }

        public IStatisticService StatisticService { get; }

        public IUserService UserService { get; }

        public RepositoryAccessContext Context => new RepositoryAccessContext();


        const string BlogOptionsEntry = nameof(BlogOptionsEntry);

        public async Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default)
        {
            var entry = await DataContext.BlogEntries.FindAsync(new object[] { BlogOptionsEntry }, cancellationToken).ConfigureAwait(false);
            if (entry is null)
            {
                return new BlogOptions();
            }
            else
            {
                return JsonSerializer.Deserialize<BlogOptions>(entry.Value) ?? throw new NullReferenceException("Options is null");
            }
        }

        public async Task<bool> SetOptions(BlogOptions options, CancellationToken cancellationToken = default)
        {
            var entry = await DataContext.BlogEntries.FindAsync(new object[] { BlogOptionsEntry }, cancellationToken).ConfigureAwait(false);
            if (entry is null)
            {
                DataContext.BlogEntries.Add(new RawEntry
                {
                    Id = BlogOptionsEntry,
                    Value = JsonSerializer.Serialize(options)
                });
            }
            else
            {
                entry.Value = JsonSerializer.Serialize(options);
            }
            await DataContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}

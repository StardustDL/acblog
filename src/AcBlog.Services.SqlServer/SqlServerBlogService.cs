using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.SqlServer.Models;
using AcBlog.Services.Generators.Sitemap;
using AcBlog.Services.Generators.Syndication;
using AcBlog.Services.Models;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

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

        public async Task<QueryResponse<string>> Query(BlogQueryRequest query, CancellationToken cancellationToken = default)
        {
            switch (query.Type)
            {
                case BlogQueryRequestStrings.Sitemap:
                    {
                        var baseAddress = query.GetBaseAddress() ?? string.Empty;
                        var siteMapBuilder = await this.BuildSitemap(baseAddress);
                        StringBuilder sb = new StringBuilder();
                        await using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { Async = true }))
                            siteMapBuilder.Build().WriteTo(writer);
                        return QueryResponse.Success(sb.ToString());
                    }
                case BlogQueryRequestStrings.AtomFeed:
                    {
                        var baseAddress = query.GetBaseAddress() ?? string.Empty;
                        var feed = await this.BuildSyndication(baseAddress);
                        StringBuilder sb = new StringBuilder();
                        using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { Async = true }))
                            feed.GetAtom10Formatter().WriteTo(writer);
                        return QueryResponse.Success(sb.ToString());
                    }
                default:
                    return QueryResponse.Error<string>();
            }
        }
    }
}

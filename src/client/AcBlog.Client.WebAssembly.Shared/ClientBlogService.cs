using AcBlog.Client.WebAssembly.Models;
using AcBlog.Data.Models;
using AcBlog.Data.Repositories.Externals;
using AcBlog.Sdk;
using AcBlog.Sdk.Api;
using AcBlog.Sdk.Extensions;
using AcBlog.Sdk.FileSystem;
using Listat;
using Loment;
using Microsoft.Extensions.Options;
using StardustDL.Extensions.FileProviders.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Client.WebAssembly
{
    public class ClientBlogService : IBlogService
    {
        public ClientBlogService(IOptions<ServerSettings> serverOptions, IHttpClientFactory httpClientFactory)
        {
            var server = serverOptions.Value;
            {
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };
                if (string.IsNullOrEmpty(server.Main.Uri))
                {
                    client.BaseAddress = new Uri($"{server.BaseAddress.TrimEnd('/')}/data/");
                    Main = new FileSystemBlogService(new HttpFileProvider(client));
                }
                else if (server.Main.IsStatic)
                {
                    if (!server.Main.Uri.EndsWith("/"))
                        server.Main.Uri += "/";
                    client.BaseAddress = new Uri(server.Main.Uri);
                    Main = new FileSystemBlogService(new HttpFileProvider(client));
                }
                else
                {
                    client.BaseAddress = new Uri(server.Main.Uri);
                    Main = new ApiBlogService(client);
                }
            }

            if (server.Comment.Enable)
            {
                if (!server.Comment.Uri.EndsWith("/"))
                    server.Comment.Uri += "/";
                var client = httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(server.Comment.Uri);
                Comment = new LomentCommentRepository(new LomentService(client)).AsService(this);
            }

            if (server.Statistic.Enable)
            {
                if (!server.Statistic.Uri.EndsWith("/"))
                    server.Statistic.Uri += "/";
                var client = httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(server.Statistic.Uri);
                Statistic = new ListatStatisticRepository(new ListatService(client)).AsService(this);
            }
        }

        private IBlogService Main { get; }

        private ICommentService Comment { get; }

        private IStatisticService Statistic { get; }

        public IPostService PostService => Main.PostService;

        public IPageService PageService => Main.PageService;

        public ILayoutService LayoutService => Main.LayoutService;

        public ICommentService CommentService => Comment ?? Main.CommentService;

        public IStatisticService StatisticService => Statistic ?? Main.StatisticService;

        public Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default) => Main.GetOptions(cancellationToken);
    }
}

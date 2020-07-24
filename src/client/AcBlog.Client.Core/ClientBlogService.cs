using AcBlog.Client.Models;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
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
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Client
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

            try
            {
                switch (server.Comment.Type)
                {
                    case CommentServerType.Loment:
                    {
                        if (!server.Comment.Uri.EndsWith("/"))
                            server.Comment.Uri += "/";
                        var client = httpClientFactory.CreateClient();
                        client.BaseAddress = new Uri(server.Comment.Uri);
                        CommentService = new LomentCommentRepository(new LomentService(client)).AsService(this);
                    }
                    break;
                    case CommentServerType.Main:
                    {
                        CommentService = Main.CommentService;
                    }
                    break;
                    case CommentServerType.Disable:
                    {
                        CommentService = null;
                    }
                    break;
                }
            }
            catch
            {
                CommentService = null;
            }

            CommentService ??= new EmptyCommentRepo().AsService(this);

            try
            {
                switch (server.Statistic.Type)
                {
                    case StatisticServerType.Listat:
                    {
                        if (!server.Statistic.Uri.EndsWith("/"))
                            server.Statistic.Uri += "/";
                        var client = httpClientFactory.CreateClient();
                        client.BaseAddress = new Uri(server.Statistic.Uri);
                        StatisticService = new ListatStatisticRepository(new ListatService(client)).AsService(this);
                    }
                    break;
                    case StatisticServerType.Main:
                    {
                        StatisticService = Main.StatisticService;
                    }
                    break;
                    case StatisticServerType.Disable:
                    {
                        StatisticService = null;
                    }
                    break;
                }
            }
            catch
            {
                StatisticService = null;
            }

            StatisticService ??= new EmptyStatisticRepo().AsService(this);
        }

        private IBlogService Main { get; }

        public IPostService PostService => Main.PostService;

        public IPageService PageService => Main.PageService;

        public ILayoutService LayoutService => Main.LayoutService;

        public ICommentService CommentService { get; }

        public IStatisticService StatisticService { get; }

        public Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default) => Main.GetOptions(cancellationToken);

        class EmptyCommentRepo : EmptyRecordRepository<Comment, string, CommentQueryRequest>, ICommentRepository
        {
        }

        class EmptyStatisticRepo : EmptyRecordRepository<Statistic, string, StatisticQueryRequest>, IStatisticRepository
        {
        }
    }
}

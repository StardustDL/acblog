using AcBlog.Data.Models;
using AcBlog.Services;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk.Api
{
    public class ApiBlogService : IBlogService
    {
        public ApiBlogService(HttpClient httpClient)
        {
            HttpClient = httpClient;

            PostService = new PostService(this, httpClient);

            PageService = new PageService(this, httpClient);

            LayoutService = new LayoutService(this, httpClient);

            CommentService = new CommentService(this, httpClient);

            StatisticService = new StatisticService(this, httpClient);
        }

        public HttpClient HttpClient { get; }

        public IPostService PostService { get; }

        public IPageService PageService { get; }

        public ILayoutService LayoutService { get; }

        public ICommentService CommentService { get; }

        public IStatisticService StatisticService { get; }

        public IFileService FileService => throw new NotImplementedException();


        protected const string PrepUrl = "/Blog";

        public async Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default)
        {
            using var responseMessage = await HttpClient.GetAsync(PrepUrl, cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<BlogOptions>(cancellationToken: cancellationToken).ConfigureAwait(false)
                ?? throw new NullReferenceException("Null");
        }
    }
}

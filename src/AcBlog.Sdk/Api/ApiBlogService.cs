using AcBlog.Data.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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
        }

        public HttpClient HttpClient { get; }

        public IPostService PostService { get; }

        public IPageService PageService { get; }

        public ILayoutService LayoutService { get; }

        public ICommentService CommentService => throw new NotImplementedException();

        public IStatisticService StatisticService => throw new NotImplementedException();

        public IFileService FileService => throw new NotImplementedException();


        const string PrepUrl = "/Blog";

        public async Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default)
        {
            using var responseMessage = await HttpClient.GetAsync(PrepUrl, cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<BlogOptions>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}

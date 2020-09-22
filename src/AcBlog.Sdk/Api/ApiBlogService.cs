using AcBlog.Data.Models;
using AcBlog.Data.Repositories;
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

            UserService = new UserService(this, httpClient);
        }

        public RepositoryAccessContext Context { get; set; } = new RepositoryAccessContext();

        internal void SetHeader()
        {
            if (Context is not null && !string.IsNullOrWhiteSpace(Context.Token))
            {
                HttpClient.DefaultRequestHeaders.Remove("Authorization");
                HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Context.Token);
            }
            else
            {
                HttpClient.DefaultRequestHeaders.Remove("Authorization");
            }
        }

        public HttpClient HttpClient { get; }

        public IPostService PostService { get; }

        public IPageService PageService { get; }

        public ILayoutService LayoutService { get; }

        public ICommentService CommentService { get; }

        public IStatisticService StatisticService { get; }

        public IUserService UserService { get; }

        public IFileService FileService => throw new NotImplementedException();


        protected const string PrepUrl = "/Blog";

        public async Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default)
        {
            SetHeader();
            using var responseMessage = await HttpClient.GetAsync($"{PrepUrl}/options", cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<BlogOptions>(cancellationToken: cancellationToken).ConfigureAwait(false)
                ?? throw new NullReferenceException("Null");
        }

        public async Task<bool> SetOptions(BlogOptions options, CancellationToken cancellationToken = default)
        {
            SetHeader();
            using var responseMessage = await HttpClient.PostAsJsonAsync($"{PrepUrl}/options", options, cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}

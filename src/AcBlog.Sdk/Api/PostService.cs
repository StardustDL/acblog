using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Services;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk.Api
{
    internal class PostService : BaseRecordApiService<Post, PostQueryRequest, IPostRepositorySearcher>, IPostService
    {
        public PostService(IBlogService blog, HttpClient httpClient) : base(blog, httpClient)
        {
            Protector = new DocumentProtector();
        }

        public IProtector<Document> Protector { get; }

        protected override string PrepUrl => "/Posts";

        public async Task<CategoryTree> GetCategories(CancellationToken cancellationToken = default)
        {
            SetHeader();
            using var responseMessage = await HttpClient.GetAsync($"{PrepUrl}/categories", cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<CategoryTree>(cancellationToken: cancellationToken).ConfigureAwait(false)
                ?? throw new NullReferenceException("Null");
        }

        public async Task<KeywordCollection> GetKeywords(CancellationToken cancellationToken = default)
        {
            SetHeader();
            using var responseMessage = await HttpClient.GetAsync($"{PrepUrl}/keywords", cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<KeywordCollection>(cancellationToken: cancellationToken).ConfigureAwait(false)
                ?? throw new NullReferenceException("Null");
        }
    }
}

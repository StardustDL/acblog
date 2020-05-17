using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.SDK.API
{
    internal abstract class BaseApiService<T, TQuery> where T : class, IHasId<string>
    {
        protected abstract string PrepUrl { get; }

        public BaseApiService(IBlogService blog, HttpClient httpClient)
        {
            BlogService = blog;
            HttpClient = httpClient;
        }

        public IBlogService BlogService { get; private set; }

        public HttpClient HttpClient { get; }

        public RepositoryAccessContext? Context { get; set; }

        private void SetHeader()
        {
            if (Context != null && !string.IsNullOrWhiteSpace(Context.Token))
            {
                HttpClient.DefaultRequestHeaders.Remove("Authorization");
                HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Context.Token);
            }
        }

        public virtual async Task<IEnumerable<string>> All(CancellationToken cancellationToken = default)
        {
            SetHeader();
            using var responseMessage = await HttpClient.GetAsync(PrepUrl, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<IEnumerable<string>>(cancellationToken: cancellationToken);
        }

        public virtual async Task<bool> CanRead(CancellationToken cancellationToken = default)
        {
            SetHeader();
            using var responseMessage = await HttpClient.GetAsync($"{PrepUrl}/actions/read", cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
        }

        public virtual async Task<bool> CanWrite(CancellationToken cancellationToken = default)
        {
            SetHeader();
            using var responseMessage = await HttpClient.GetAsync($"{PrepUrl}/actions/write", cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
        }

        public virtual async Task<string?> Create(T value, CancellationToken cancellationToken = default)
        {
            SetHeader();

            using var responseMessage = await HttpClient.PostAsJsonAsync(PrepUrl, value, cancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
                return null;

            return await responseMessage.Content.ReadAsStringAsync();
        }

        public virtual async Task<bool> Delete(string id, CancellationToken cancellationToken = default)
        {
            SetHeader();

            using var responseMessage = await HttpClient.DeleteAsync($"{PrepUrl}/{id}", cancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
                return false;

            return await responseMessage.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
        }

        public virtual async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            SetHeader();

            using var responseMessage = await HttpClient.GetAsync($"{PrepUrl}/{id}", cancellationToken);
            return responseMessage.IsSuccessStatusCode;
        }

        public virtual async Task<T?> Get(string id, CancellationToken cancellationToken = default)
        {
            SetHeader();

            using var responseMessage = await HttpClient.GetAsync($"{PrepUrl}/{id}", cancellationToken);
            responseMessage.EnsureSuccessStatusCode();

            var result = await responseMessage.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
            if (result != null)
                result.Id = id;

            return result;
        }

        public virtual async Task<QueryResponse<string>> Query(TQuery query, CancellationToken cancellationToken = default)
        {
            SetHeader();

            using var responseMessage = await HttpClient.PutAsJsonAsync($"{PrepUrl}/query", query, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<QueryResponse<string>>(cancellationToken: cancellationToken);
        }

        public virtual async Task<bool> Update(T value, CancellationToken cancellationToken = default)
        {
            SetHeader();

            using var responseMessage = await HttpClient.PutAsJsonAsync($"{PrepUrl}/{value.Id}", value, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
        }
    }
}

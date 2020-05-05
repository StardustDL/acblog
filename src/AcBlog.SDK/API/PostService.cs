using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AcBlog.SDK.API
{
    internal class PostService : IPostService
    {
        const string _prepUrl = "/Posts";

        public PostService(IBlogService blog, HttpClient httpClient)
        {
            BlogService = blog;
            HttpClient = httpClient;
        }

        public IBlogService BlogService { get; private set; }

        public HttpClient HttpClient { get; }

        public RepositoryAccessContext? Context { get; set; }

        public IProtector<Post> Protector => throw new System.NotImplementedException();

        public async Task<IEnumerable<string>> All()
        {
            using var responseMessage = await HttpClient.GetAsync(_prepUrl);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<IEnumerable<string>>();
        }

        public async Task<bool> CanRead()
        {
            using var responseMessage = await HttpClient.GetAsync($"{_prepUrl}/actions/read");
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<bool> CanWrite()
        {
            using var responseMessage = await HttpClient.GetAsync($"{_prepUrl}/actions/write");
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<string?> Create(Post value)
        {
            using var responseMessage = await HttpClient.PostAsJsonAsync(_prepUrl, value);

            if (!responseMessage.IsSuccessStatusCode)
                return null;

            var id = await responseMessage.Content.ReadAsStringAsync();
            value.Id = id;
            return id;
        }

        public async Task<bool> Delete(string id)
        {
            using var responseMessage = await HttpClient.DeleteAsync($"{_prepUrl}/{id}");

            if (!responseMessage.IsSuccessStatusCode)
                return false;

            return await responseMessage.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<bool> Exists(string id)
        {
            using var responseMessage = await HttpClient.GetAsync($"{_prepUrl}/{id}");
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<Post?> Get(string id)
        {
            using var responseMessage = await HttpClient.GetAsync($"{_prepUrl}/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<Post>();
        }

        public async Task<QueryResponse<string>> Query(PostQueryRequest query)
        {
            using var responseMessage = await HttpClient.PutAsJsonAsync($"{_prepUrl}/query", query);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<QueryResponse<string>>();
        }

        public async Task<bool> Update(Post value)
        {
            using var responseMessage = await HttpClient.PutAsJsonAsync($"{_prepUrl}/{value.Id}", value);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<bool>();
        }
    }
}

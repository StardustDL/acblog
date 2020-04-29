using AcBlog.Data.Models;
using AcBlog.Data.Providers;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AcBlog.SDK.API
{
    internal class PostService : IPostService
    {
        const string PrepUrl = "/Posts";

        public PostService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }

        public bool IsReadable => true;

        public bool IsWritable => true;

        public ProviderContext? Context { get; set; }

        public async IAsyncEnumerable<Post> All()
        {
            var responseMessage = await HttpClient.GetAsync(PrepUrl);
            responseMessage.EnsureSuccessStatusCode();
            foreach (var v in await responseMessage.Content.ReadFromJsonAsync<Post[]>())
                yield return v;
        }

        public async Task<string?> Create(Post value)
        {
            var responseMessage = await HttpClient.PostAsJsonAsync(PrepUrl, value);

            if (!responseMessage.IsSuccessStatusCode)
                return null;

            var id = await responseMessage.Content.ReadAsStringAsync();
            value.Id = id;
            return id;
        }

        public async Task<bool> Delete(string id)
        {
            var responseMessage = await HttpClient.DeleteAsync($"{PrepUrl}/{id}");

            if (!responseMessage.IsSuccessStatusCode)
                return false;

            return await responseMessage.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<bool> Exists(string id)
        {
            var responseMessage = await HttpClient.GetAsync($"{PrepUrl}/{id}");
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<Post> Get(string id)
        {
            var responseMessage = await HttpClient.GetAsync($"{PrepUrl}/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<Post>();
        }

        public async Task<bool> Update(Post value)
        {
            var responseMessage = await HttpClient.PutAsJsonAsync($"{PrepUrl}/{value.Id}", value);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<bool>();
        }
    }
}

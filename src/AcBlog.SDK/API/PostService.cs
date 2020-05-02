using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Providers;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AcBlog.SDK.API
{
    internal class PostService : IPostService
    {
        const string _prepUrl = "/Posts";

        public PostService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }

        public bool IsReadable => true;

        public bool IsWritable => true;

        public ProviderContext? Context { get; set; }

        public async Task<IEnumerable<Post>> All()
        {
            var responseMessage = await HttpClient.GetAsync(_prepUrl);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<IEnumerable<Post>>();
        }

        public async Task<string?> Create(Post value)
        {
            var responseMessage = await HttpClient.PostAsJsonAsync(_prepUrl, value);

            if (!responseMessage.IsSuccessStatusCode)
                return null;

            var id = await responseMessage.Content.ReadAsStringAsync();
            value.Id = id;
            return id;
        }

        public async Task<bool> Delete(string id)
        {
            var responseMessage = await HttpClient.DeleteAsync($"{_prepUrl}/{id}");

            if (!responseMessage.IsSuccessStatusCode)
                return false;

            return await responseMessage.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<bool> Exists(string id)
        {
            var responseMessage = await HttpClient.GetAsync($"{_prepUrl}/{id}");
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<Post?> Get(string id)
        {
            var responseMessage = await HttpClient.GetAsync($"{_prepUrl}/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<Post>();
        }

        public Task<PostQueryResponse> Query(PostQueryRequest query)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Update(Post value)
        {
            var responseMessage = await HttpClient.PutAsJsonAsync($"{_prepUrl}/{value.Id}", value);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<bool>();
        }
    }
}

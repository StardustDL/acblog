using AcBlog.Data.Models;
using AcBlog.Data.Providers;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AcBlog.SDK.API
{
    internal class UserService : IUserService
    {
        const string PrepUrl = "/Users";

        public UserService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }

        public bool IsReadable => true;

        public bool IsWritable => true;

        public ProviderContext? Context { get; set; }

        public async IAsyncEnumerable<User> All()
        {
            var responseMessage = await HttpClient.GetAsync(PrepUrl);
            responseMessage.EnsureSuccessStatusCode();
            foreach(var v in await responseMessage.Content.ReadFromJsonAsync<User[]>())
                yield return v;
        }

        public async Task<string?> Create(User value)
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

        public async Task<User> Get(string id)
        {
            var responseMessage = await HttpClient.GetAsync($"{PrepUrl}/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<User>();
        }

        public async Task<bool> Update(User value)
        {
            var responseMessage = await HttpClient.PutAsJsonAsync($"{PrepUrl}/{value.Id}", value);

            if (!responseMessage.IsSuccessStatusCode)
                return false;

            return await responseMessage.Content.ReadFromJsonAsync<bool>();
        }
    }
}

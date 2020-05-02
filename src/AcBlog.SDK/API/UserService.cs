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
        const string _prepUrl = "/Users";

        public UserService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }

        public bool IsReadable => true;

        public bool IsWritable => true;

        public ProviderContext? Context { get; set; }

        public async Task<IEnumerable<User>> All()
        {
            var responseMessage = await HttpClient.GetAsync(_prepUrl);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<IEnumerable<User>>();
        }

        public async Task<string?> Create(User value)
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

        public async Task<User?> Get(string id)
        {
            var responseMessage = await HttpClient.GetAsync($"{_prepUrl}/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadFromJsonAsync<User>();
        }

        public async Task<bool> Update(User value)
        {
            var responseMessage = await HttpClient.PutAsJsonAsync($"{_prepUrl}/{value.Id}", value);

            if (!responseMessage.IsSuccessStatusCode)
                return false;

            return await responseMessage.Content.ReadFromJsonAsync<bool>();
        }
    }
}

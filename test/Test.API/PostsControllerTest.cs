using AcBlog.Data.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Test.API
{
    [TestClass]
    public class PostsControllerTest
    {
        const string PREP = "/Posts";

        WebApplicationFactory<AcBlog.Server.API.Program> Factory { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Factory = new WebApplicationFactory<AcBlog.Server.API.Program>();
        }

        [TestMethod]
        public async Task All()
        {
            using var client = Factory.CreateClient();
            var responseMessage = await client.GetAsync(PREP);
            responseMessage.EnsureSuccessStatusCode();
            var result = await responseMessage.Content.ReadFromJsonAsync<Post[]>();
        }

        [TestMethod]
        public async Task CRUD()
        {
            using var client = Factory.CreateClient();

            Post origin = new Post { Title = "title" };

            // Create

            var responseMessage = await client.PostAsJsonAsync(PREP, origin);

            responseMessage.EnsureSuccessStatusCode();

            var id = await responseMessage.Content.ReadAsStringAsync();

            // Get

            responseMessage = await client.GetAsync($"{PREP}/{id}");
            responseMessage.EnsureSuccessStatusCode();

            var result = await responseMessage.Content.ReadFromJsonAsync<Post>();
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(origin.Title, result.Title);

            // Update

            origin.Title = "new title";
            responseMessage = await client.PutAsJsonAsync($"{PREP}/{id}", origin);

            responseMessage.EnsureSuccessStatusCode();

            Assert.IsTrue(await responseMessage.Content.ReadFromJsonAsync<bool>());

            responseMessage = await client.GetAsync($"{PREP}/{id}");
            responseMessage.EnsureSuccessStatusCode();
            result = await responseMessage.Content.ReadFromJsonAsync<Post>();
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(origin.Title, result.Title);

            // Delete
            responseMessage = await client.DeleteAsync($"{PREP}/{id}");
            Assert.IsTrue(await responseMessage.Content.ReadFromJsonAsync<bool>());
            responseMessage = await client.GetAsync($"{PREP}/{id}");
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, responseMessage.StatusCode);
        }

        [TestCleanup]
        public void Clean()
        {
            Factory.Dispose();
        }
    }
}

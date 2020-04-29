using AcBlog.Data.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Test.API
{
    [TestClass]
    public class UsersControllerTest
    {
        const string PREP = "/Users";

        WebApplicationFactory<AcBlog.Server.API.Program> Factory { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Factory = new WebApplicationFactory<AcBlog.Server.API.Program>();
        }

        [TestCleanup]
        public void Clean()
        {
            Factory.Dispose();
        }

        [TestMethod]
        public async Task All()
        {
            using var client = Factory.CreateClient();
            var responseMessage = await client.GetAsync(PREP);
            responseMessage.EnsureSuccessStatusCode();
            var result = await responseMessage.Content.ReadFromJsonAsync<User[]>();
        }

        [TestMethod]
        public async Task CRUD()
        {
            using var client = Factory.CreateClient();

            User origin = new User { Nickname = "nick" };

            // Create

            var responseMessage = await client.PostAsJsonAsync(PREP, origin);

            responseMessage.EnsureSuccessStatusCode();

            var id = await responseMessage.Content.ReadAsStringAsync();

            // Get

            responseMessage = await client.GetAsync($"{PREP}/{id}");
            responseMessage.EnsureSuccessStatusCode();

            var result = await responseMessage.Content.ReadFromJsonAsync<User>();
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(origin.Nickname, result.Nickname);

            // Update

            origin.Nickname = "new nickname";
            responseMessage = await client.PutAsJsonAsync($"{PREP}/{id}", origin);

            responseMessage.EnsureSuccessStatusCode();

            Assert.IsTrue(await responseMessage.Content.ReadFromJsonAsync<bool>());

            responseMessage = await client.GetAsync($"{PREP}/{id}");
            responseMessage.EnsureSuccessStatusCode();
            result = await responseMessage.Content.ReadFromJsonAsync<User>();
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(origin.Nickname, result.Nickname);

            // Delete
            responseMessage = await client.DeleteAsync($"{PREP}/{id}");
            responseMessage.EnsureSuccessStatusCode();
            Assert.IsTrue(await responseMessage.Content.ReadFromJsonAsync<bool>());
            responseMessage = await client.GetAsync($"{PREP}/{id}");
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, responseMessage.StatusCode);
        }
    }
}

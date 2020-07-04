using AcBlog.Data.Models;
using AcBlog.SDK.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.SDK
{
    [TestClass]
    public class ApiTest : SDKTest
    {
        WebApplicationFactory<AcBlog.Server.API.Program> Factory { get; set; }

        ApiBlogService Service { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Factory = new WebApplicationFactory<AcBlog.Server.API.Program>();
            Service = new ApiBlogService(Factory.CreateClient());
        }

        [TestCleanup]
        public void Clean()
        {
            Service.HttpClient.Dispose();
            Factory.Dispose();
        }

        public Task User() => UserService(Service.UserService);

        public Task Post() => PostService(Service.PostService);
    }
}

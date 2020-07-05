using AcBlog.Data.Models;
using AcBlog.Sdk.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.SDK
{
    [TestClass]
    public class ApiTest : SdkTest
    {
        WebApplicationFactory<AcBlog.Server.Api.Program> Factory { get; set; }

        ApiBlogService Service { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Factory = new WebApplicationFactory<AcBlog.Server.Api.Program>();
            Service = new ApiBlogService(Factory.CreateClient());
        }

        [TestCleanup]
        public void Clean()
        {
            Service.HttpClient.Dispose();
            Factory.Dispose();
        }

        public Task Post() => PostService(Service.PostService);
    }
}

using AcBlog.SDK.StaticFile;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.SDK
{
    [TestClass]
    public class StaticFileTest : SDKTest
    {
        HttpStaticFileBlogService Service { get; set; }

        WebApplicationFactory<AcBlog.Server.API.Program> Factory { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Factory = new WebApplicationFactory<AcBlog.Server.API.Program>();
            Service = new HttpStaticFileBlogService("/data", Factory.CreateClient());
        }

        [TestCleanup]
        public void Clean()
        {
            Service.HttpClient.Dispose();
            Factory.Dispose();
        }

        public Task User() => UserService(Service.UserService);

        public Task Article() => PostService(Service.ArticleService);

        public Task Slides() => PostService(Service.SlidesService);
    }
}

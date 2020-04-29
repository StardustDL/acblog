using AcBlog.Data.Providers.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test.Data.Providers
{
    [TestClass]
    public class FileSystemTest : ProviderTest
    {
        string RootPath { get; set; }

        [TestInitialize]
        public void Setup()
        {
            RootPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "test");
            if (Directory.Exists(RootPath))
                Directory.Delete(RootPath, true);
            Directory.CreateDirectory(RootPath);
        }

        [TestCleanup]
        public void Clean()
        {
            if (Directory.Exists(RootPath))
                Directory.Delete(RootPath, true);
        }

        [TestMethod]
        public Task User()
        {
            var root = Path.Join(RootPath, "users");
            Directory.CreateDirectory(root);

            UserProvider provider = new UserProvider(root);

            return UserProvider(provider);
        }

        [TestMethod]
        public Task Post()
        {
            var root = Path.Join(RootPath, "posts");
            Directory.CreateDirectory(root);

            PostProvider provider = new PostProvider(root);

            return PostProvider(provider);
        }
    }
}

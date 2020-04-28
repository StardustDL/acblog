using AcBlog.Data.Models;
using AcBlog.Data.Providers;
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
    public class FileSystemTest
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
        public async Task User()
        {
            var root = Path.Join(RootPath, "users");
            Directory.CreateDirectory(root);

            UserProvider provider = new UserProvider(root);

            Assert.IsTrue(provider.IsReadable);
            Assert.IsTrue(provider.IsWritable);

            User user = new User
            {
                Nickname = "nick"
            };
            var id = await provider.Create(user);
            Assert.IsNotNull(id);
            Assert.IsTrue(await provider.Exists(id));

            var userLoaded = await provider.Get(id);
            Assert.AreEqual(user.Nickname, userLoaded.Nickname);

            user.Nickname = "new";
            Assert.IsTrue(await provider.Update(user));
            userLoaded = await provider.Get(id);
            Assert.AreEqual(user.Nickname, userLoaded.Nickname);

            Assert.IsTrue(await provider.Delete(id));
            Assert.IsFalse(await provider.Delete(id));
            Assert.IsFalse(await provider.Exists(id));
        }

        [TestMethod]
        public async Task Post()
        {
            var root = Path.Join(RootPath, "posts");
            Directory.CreateDirectory(root);

            PostProvider provider = new PostProvider(root);

            Assert.IsTrue(provider.IsReadable);
            Assert.IsTrue(provider.IsWritable);

            Post post = new Post
            {
                Title = "title"
            };
            var id = await provider.Create(post);
            Assert.IsNotNull(id);
            Assert.IsTrue(await provider.Exists(id));
            var postLoaded = await provider.Get(id);
            Assert.AreEqual(post.Title, postLoaded.Title);

            post.Title = "new";
            Assert.IsTrue(await provider.Update(post));
            postLoaded = await provider.Get(id);
            Assert.AreEqual(post.Title, postLoaded.Title);

            Assert.IsTrue(await provider.Delete(id));
            Assert.IsFalse(await provider.Delete(id));
            Assert.IsFalse(await provider.Exists(id));
        }
    }
}

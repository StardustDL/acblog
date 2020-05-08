using AcBlog.Data.Models;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using AcBlog.Data.Repositories.FileSystem.Readers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test.Data.Repositories
{
    [TestClass]
    public class FileSystemTest : RepositoriyTest
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

            var seedUser = new User[]
            {
                new User{Nickname = "a", Id = Guid.NewGuid().ToString()},
            };

            await UserRepositoryBuilder.Build(seedUser, root, 10);

            IUserRepository provider = new UserLocalReader(root);

            await UserRepository(provider);
        }

        [TestMethod]
        public async Task Post()
        {
            var root = Path.Join(RootPath, "posts");
            Directory.CreateDirectory(root);

            var seedPost = new Post[]
            {
                new Post{Title = "a", Id = Guid.NewGuid().ToString()},
            };

            await PostRepositoryBuilder.Build(seedPost.Select<Post, (Post, ProtectionKey)>(x => (x, null)).ToList(), new PostProtector(), root, 10);

            IPostRepository provider = new PostLocalReader(root);

            await PostRepository(provider);
        }
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Data.Repositories
{
    public abstract class RepositoriyTest
    {
        protected async Task UserRepository(IUserRepository repository)
        {
            bool canRead = await repository.CanRead();
            bool canWrite = await repository.CanWrite();
            if (canRead)
            {
                foreach (var id in await repository.All())
                {
                    var user = await repository.Get(id);
                    Assert.IsNotNull(user);
                }
            }
            if (canWrite)
            {
                User user = new User
                {
                    Nickname = "nick"
                };
                var id = await repository.Create(user);
                Assert.IsNotNull(id);

                if (canRead)
                {
                    Assert.IsTrue(await repository.Exists(id));
                    var userLoaded = await repository.Get(id);
                    Assert.AreEqual(user.Nickname, userLoaded.Nickname);
                }

                user.Nickname = "new";
                Assert.IsTrue(await repository.Update(user));

                if (canRead)
                {
                    var userLoaded = await repository.Get(id);
                    Assert.AreEqual(user.Nickname, userLoaded.Nickname);
                }

                Assert.IsTrue(await repository.Delete(id));
                Assert.IsFalse(await repository.Delete(id));

                if (canRead)
                {
                    Assert.IsFalse(await repository.Exists(id));
                }
            }
        }

        protected async Task PostRepository(IPostRepository repository)
        {
            bool canRead = await repository.CanRead();
            bool canWrite = await repository.CanWrite();
            if (canRead)
            {
                foreach (var id in await repository.All())
                {
                    var user = await repository.Get(id);
                }
            }
            if (canWrite)
            {
                Post user = new Post
                {
                    Title = "nick"
                };
                var id = await repository.Create(user);
                Assert.IsNotNull(id);

                if (canRead)
                {
                    Assert.IsTrue(await repository.Exists(id));
                    var userLoaded = await repository.Get(id);
                    Assert.AreEqual(user.Title, userLoaded.Title);
                }

                user.Title = "new";
                Assert.IsTrue(await repository.Update(user));

                if (canRead)
                {
                    var userLoaded = await repository.Get(id);
                    Assert.AreEqual(user.Title, userLoaded.Title);
                }

                Assert.IsTrue(await repository.Delete(id));
                Assert.IsFalse(await repository.Delete(id));

                if (canRead)
                {
                    Assert.IsFalse(await repository.Exists(id));
                }
            }
        }
    }
}

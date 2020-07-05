using AcBlog.Data.Models;
using AcBlog.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Data.Repositories
{
    public abstract class RepositoriyTest
    {
        protected async Task PostRepository(IPostRepository repository)
        {
            var status = await repository.GetStatus();
            if (status.CanRead)
            {
                foreach (var id in await repository.All())
                {
                    var user = await repository.Get(id);
                }
            }
            if (status.CanWrite)
            {
                Post user = new Post
                {
                    Title = "nick"
                };
                var id = await repository.Create(user);
                Assert.IsNotNull(id);

                if (status.CanRead)
                {
                    Assert.IsTrue(await repository.Exists(id));
                    var userLoaded = await repository.Get(id);
                    Assert.AreEqual(user.Title, userLoaded.Title);
                }

                user.Title = "new";
                Assert.IsTrue(await repository.Update(user));

                if (status.CanRead)
                {
                    var userLoaded = await repository.Get(id);
                    Assert.AreEqual(user.Title, userLoaded.Title);
                }

                Assert.IsTrue(await repository.Delete(id));
                Assert.IsFalse(await repository.Delete(id));

                if (status.CanRead)
                {
                    Assert.IsFalse(await repository.Exists(id));
                }
            }
        }
    }
}

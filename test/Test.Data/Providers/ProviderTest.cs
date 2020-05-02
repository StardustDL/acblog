using AcBlog.Data.Models;
using AcBlog.Data.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Data.Providers
{
    public abstract class ProviderTest
    {
        protected async Task UserProvider(IUserProvider provider)
        {
            Assert.IsTrue(provider.IsReadable);
            Assert.IsTrue(provider.IsWritable);

            foreach (var _ in await provider.All()) ;

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

        protected async Task PostProvider(IPostProvider provider)
        {
            Assert.IsTrue(provider.IsReadable);
            Assert.IsTrue(provider.IsWritable);

            foreach (var _ in await provider.All()) ;

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

using AcBlog.Data.Protections;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Test.Data.Protections
{
    public static class ProtectorTester
    {
        public static async Task TestPassword<T>(this IProtector<T> protector, T raw)
        {
            ProtectionKey key = new ProtectionKey
            {
                Password = "123"
            };
            var pro = await protector.Protect(raw, key);

            Assert.IsFalse(await protector.IsProtected(raw));
            Assert.IsTrue(await protector.IsProtected(pro));

            var depro = await protector.Deprotect(pro, key);
            Assert.IsFalse(await protector.IsProtected(depro));
            depro.ShouldDeepEqual(raw);

            await Assert.ThrowsExceptionAsync<ProtectionException>(() => protector.Deprotect(pro, new ProtectionKey
            {
                Password = "abc"
            }));
        }
    }
}

using AcBlog.Data.Protections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Test.Data.Protections
{
    [TestClass]
    public class DocumentProtectorTest
    {
        [TestMethod]
        public async Task Password()
        {
            await new DocumentProtector().TestPassword(Generator.GetDocument());
        }
    }
}

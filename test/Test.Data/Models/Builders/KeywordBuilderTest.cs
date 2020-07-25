using AcBlog.Data.Models;
using AcBlog.Data.Models.Builders;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Data.Models.Builders
{
    [TestClass]
    public class KeywordBuilderTest
    {
        [TestMethod]
        public void Basic()
        {
            KeywordBuilder builder = new KeywordBuilder();
            builder.AddKeyword("a", "b");
            builder.Build().ShouldDeepEqual(new Keyword(new[] { "a", "b" }));

            builder.AddKeyword("c");
            builder.Build().ShouldDeepEqual(new Keyword(new[] { "a", "b", "c" }));

            builder.RemoveKeyword("c");
            builder.Build().ShouldDeepEqual(new Keyword(new[] { "a", "b" }));

            try
            {
                builder.AddKeyword($"a{Keyword.KeywordSeperator}b");
                Assert.Fail("Invalid name");
            }
            catch
            {

            }
        }
    }
}

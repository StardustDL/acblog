using AcBlog.Data.Models;
using AcBlog.Data.Models.Builders;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            builder.Build().ShouldDeepEqual(new Keyword{ Items = new[] { "a", "b" }});

            builder.AddKeyword("c");
            builder.Build().ShouldDeepEqual(new Keyword{ Items = new[] { "a", "b", "c" }});

            builder.RemoveKeyword("c");
            builder.Build().ShouldDeepEqual(new Keyword{ Items = new[] { "a", "b" }});

            Assert.ThrowsException<Exception>(() => builder.AddKeyword($"a;b"));
        }

        [TestMethod]
        public void String()
        {
            _ = Generator.GetKeyword().ToString();
        }

    }
}

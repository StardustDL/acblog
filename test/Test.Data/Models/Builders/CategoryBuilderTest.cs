using AcBlog.Data.Models;
using AcBlog.Data.Models.Builders;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test.Data.Models.Builders
{
    [TestClass]
    public class CategoryBuilderTest
    {
        [TestMethod]
        public void Basic()
        {
            CategoryBuilder builder = new CategoryBuilder();
            builder.AddSubCategory("a", "b");
            builder.Build().ShouldDeepEqual(new Category { Items = new[] { "a", "b" } });

            builder.AddSubCategory("c");
            builder.Build().ShouldDeepEqual(new Category { Items = new[] { "a", "b", "c" } });

            builder.RemoveSubCategory();
            builder.Build().ShouldDeepEqual(new Category { Items = new[] { "a", "b" } });

            Assert.ThrowsException<Exception>(() => builder.AddSubCategory($"a/b"));
        }

        [TestMethod]
        public void String()
        {
            _ = Generator.GetCategory().ToString();
        }
    }
}

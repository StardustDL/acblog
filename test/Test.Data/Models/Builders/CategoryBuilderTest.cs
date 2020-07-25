using AcBlog.Data.Models;
using AcBlog.Data.Models.Builders;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

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
            builder.Build().ShouldDeepEqual(new Category(new[] { "a", "b" }));

            builder.AddSubCategory("c");
            builder.Build().ShouldDeepEqual(new Category(new[] { "a", "b", "c" }));

            builder.RemoveSubCategory();
            builder.Build().ShouldDeepEqual(new Category(new[] { "a", "b" }));

            Assert.ThrowsException<Exception>(() => builder.AddSubCategory($"a{Category.CategorySeperator}b"));
        }

        [TestMethod]
        public void String()
        {
            var item = Generator.GetCategory().ToString();
        }
    }
}

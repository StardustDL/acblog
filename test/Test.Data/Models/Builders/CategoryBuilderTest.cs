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

            try
            {
                builder.AddSubCategory($"a{Category.CategorySeperator}b");
                Assert.Fail("Invalid name");
            }
            catch
            {

            }
        }
    }

    [TestClass]
    public class FeatureBuilderTest
    {
        [TestMethod]
        public void Basic()
        {
            FeatureBuilder builder = new FeatureBuilder();
            builder.AddFeature("a", "b");
            builder.Build().ShouldDeepEqual(new Feature(new[] { "a", "b" }));

            builder.AddFeature("c");
            builder.Build().ShouldDeepEqual(new Feature(new[] { "a", "b", "c" }));

            builder.RemoveFeature("c");
            builder.Build().ShouldDeepEqual(new Feature(new[] { "a", "b" }));

            try
            {
                builder.AddFeature($"a{Feature.FeatureSeperator}b");
                Assert.Fail("Invalid name");
            }
            catch
            {

            }
        }
    }
}

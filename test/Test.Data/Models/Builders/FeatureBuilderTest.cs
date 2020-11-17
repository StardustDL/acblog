using AcBlog.Data.Models;
using AcBlog.Data.Models.Builders;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test.Data.Models.Builders
{
    [TestClass]
    public class FeatureBuilderTest
    {
        [TestMethod]
        public void Basic()
        {
            FeatureBuilder builder = new FeatureBuilder();
            builder.AddFeature("a", "b");
            builder.Build().ShouldDeepEqual(new Feature{ Items = new[] { "a", "b" }});

            builder.AddFeature("c");
            builder.Build().ShouldDeepEqual(new Feature{ Items = new[] { "a", "b", "c" }});

            builder.RemoveFeature("c");
            builder.Build().ShouldDeepEqual(new Feature{ Items = new[] { "a", "b" }});

            Assert.ThrowsException<Exception>(() => builder.AddFeature($"a,b"));
        }

        [TestMethod]
        public void String()
        {
            _ = Generator.GetFeature().ToString();
        }
    }
}

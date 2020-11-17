using System;
using System.Collections.Generic;
using System.Linq;

namespace AcBlog.Data.Models.Builders
{
    public class FeatureBuilder
    {
        HashSet<string> Inner { get; set; } = new HashSet<string>();

        public FeatureBuilder AddFeature(params string[] names)
        {
            foreach (var name in names)
            {
                if (!name.Contains(','))
                {
                    Inner.Add(name);
                }
                else
                {
                    throw new Exception($"Invalid feature name: {name}.");
                }
            }
            return this;
        }

        public FeatureBuilder RemoveFeature(params string[] names)
        {
            foreach (var name in names)
                Inner.Remove(name);
            return this;
        }

        public bool IsEmpty => Inner.Count > 0;

        public Feature Build() => new Feature { Items = Inner.ToArray() };

        public static Feature FromString(string source)
        {
            return new Feature { Items = source.Split(',', StringSplitOptions.RemoveEmptyEntries) };
        }
    }
}

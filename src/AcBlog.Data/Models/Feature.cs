using AcBlog.Data.Models.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AcBlog.Data.Models
{
    public class Feature
    {
        public static Feature Empty => new Feature();

        public const char FeatureSeperator = ',';

        public static bool IsValidName(string name) => !name.Contains(FeatureSeperator);

        public Feature() { }

        public Feature(IList<string> items) => Items = items;

        public Feature(string name) : this(new string[] { name }) { }

        public string OneName() => Items.First();

        public string OneNameOrDefault() => Items.FirstOrDefault();

        public IList<string> Items { get; set; } = Array.Empty<string>();

        public override string ToString() => string.Join(FeatureSeperator, Items);

        public static Feature Parse(string input)
        {
            var items = input.Split(FeatureSeperator, StringSplitOptions.RemoveEmptyEntries);
            FeatureBuilder builder = new FeatureBuilder();
            foreach (var name in items)
                builder.AddFeature(name);
            return builder.Build();
        }

        public static bool TryParse(string input, [NotNullWhen(true)] out Feature? feature)
        {
            try
            {
                feature = Parse(input);
                return true;
            }
            catch
            {
                feature = null;
                return false;
            }
        }
    }
}

using AcBlog.Data.Models.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Encodings.Web;

namespace AcBlog.Data.Models
{
    public class Category
    {
        public static Category Empty => new Category();

        public const char CategorySeperator = '/';

        public Category() { }

        public Category(IEnumerable<string> items) => Items = items;

        public static bool IsValidName(string name) => !name.Contains(CategorySeperator);

        public IEnumerable<string> Items { get; set; } = Array.Empty<string>();

        public override string ToString() => string.Join(CategorySeperator, Items);

        public static Category Parse(string input)
        {
            var items = input.Split(CategorySeperator, StringSplitOptions.RemoveEmptyEntries);
            CategoryBuilder builder = new CategoryBuilder();
            foreach (var name in items)
                builder.AddSubCategory(name);
            return builder.Build();
        }

        public static bool TryParse(string input, [NotNullWhen(true)] out Category? category)
        {
            try
            {
                category = Parse(input);
                return true;
            }
            catch
            {
                category = null;
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace AcBlog.Data.Models.Builders
{
    public class CategoryBuilder
    {
        List<string> Inner { get; set; } = new List<string>();

        public CategoryBuilder AddSubCategory(string name)
        {
            if (Category.IsValidName(name))
            {
                Inner.Add(name);
                return this;
            }
            else
            {
                throw new Exception($"Invalid category name: {name}.");
            }
        }

        public CategoryBuilder RemoveSubCategory()
        {
            if (Inner.Count > 0)
            {
                Inner.RemoveAt(Inner.Count);
                return this;
            }
            else
            {
                throw new Exception($"Empty category.");
            }
        }

        public bool IsEmpty => Inner.Count > 0;

        public Category Build() => new Category { Items = Inner.AsEnumerable() };
    }
}

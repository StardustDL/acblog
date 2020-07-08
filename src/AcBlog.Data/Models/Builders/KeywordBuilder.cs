using System;
using System.Collections.Generic;
using System.Linq;

namespace AcBlog.Data.Models.Builders
{
    public class KeywordBuilder
    {
        HashSet<string> Inner { get; set; } = new HashSet<string>();

        public KeywordBuilder AddKeyword(params string[] names)
        {
            foreach (var name in names)
            {
                if (Keyword.IsValidName(name))
                {
                    Inner.Add(name);
                }
                else
                {
                    throw new Exception($"Invalid keyword name: {name}.");
                }
            }
            return this;
        }

        public KeywordBuilder RemoveKeyword(params string[] names)
        {
            foreach (var name in names)
                Inner.Remove(name);
            return this;
        }

        public bool IsEmpty => Inner.Count > 0;

        public Keyword Build() => new Keyword { Items = Inner.ToArray() };
    }
}

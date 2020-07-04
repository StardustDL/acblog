using System;
using System.Collections.Generic;
using System.Linq;

namespace AcBlog.Data.Models.Builders
{
    public class KeywordBuilder
    {
        HashSet<string> Inner { get; set; } = new HashSet<string>();

        public KeywordBuilder AddKeyword(string name)
        {
            if (Keyword.IsValidName(name))
            {
                Inner.Add(name);
                return this;
            }
            else
            {
                throw new Exception($"Invalid keyword name: {name}.");
            }
        }

        public KeywordBuilder RemoveKeyword(string name)
        {
            Inner.Remove(name);
            return this;
        }

        public bool IsEmpty => Inner.Count > 0;

        public Keyword Build() => new Keyword { Items = Inner.AsEnumerable() };
    }
}

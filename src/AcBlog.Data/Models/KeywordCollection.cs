using System;
using System.Collections.Generic;

namespace AcBlog.Data.Models
{
    public class KeywordCollection
    {
        public KeywordCollection() : this(Array.Empty<Keyword>()) { }

        public KeywordCollection(IList<Keyword> items) => Items = items;

        public IList<Keyword> Items { get; set; }
    }
}

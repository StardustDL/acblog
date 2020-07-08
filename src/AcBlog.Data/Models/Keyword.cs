using AcBlog.Data.Models.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AcBlog.Data.Models
{
    public class Keyword
    {
        public static Keyword Empty => new Keyword();

        public const char KeywordSeperator = ';';

        public static bool IsValidName(string name) => !name.Contains(KeywordSeperator);

        public Keyword() { }

        public Keyword(IList<string> items) => Items = items;

        public IList<string> Items { get; set; } = Array.Empty<string>();

        public override string ToString() => string.Join(KeywordSeperator, Items);

        public static Keyword Parse(string input)
        {
            var items = input.Split(KeywordSeperator, StringSplitOptions.RemoveEmptyEntries);
            KeywordBuilder builder = new KeywordBuilder();
            foreach (var name in items)
                builder.AddKeyword(name);
            return builder.Build();
        }

        public static bool TryParse(string input, [NotNullWhen(true)] out Keyword? category)
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

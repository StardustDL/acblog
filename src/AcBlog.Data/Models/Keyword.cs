using AcBlog.Data.Models.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AcBlog.Data.Models
{
    public record Keyword
    {
        public IList<string> Items { get; init; } = Array.Empty<string>();

        public static Keyword Empty => new Keyword();
    }
}

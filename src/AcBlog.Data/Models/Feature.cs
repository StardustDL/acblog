using AcBlog.Data.Models.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AcBlog.Data.Models
{
    public record Feature
    {
        public IList<string> Items { get; init; } = Array.Empty<string>();

        public static Feature Empty => new Feature();
    }
}

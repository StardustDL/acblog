using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AcBlog.Services.Models
{
    public class BlogQueryRequest
    {
        public string Type { get; set; } = string.Empty;

        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }

    public static class BlogQueryRequestStrings
    {
        public static string? GetBaseAddress(this BlogQueryRequest request)
        {
            return request.Data.GetValueOrDefault(BaseAddress);
        }

        public const string AtomFeed = "atom";

        public const string Sitemap = "sitemap";

        public const string BaseAddress = "baseAddress";
    }
}

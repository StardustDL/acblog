using System;

namespace AcBlog.Services.Generators.Sitemap
{
    public class SitemapUrl
    {
        public string Url { get; set; } = string.Empty;
        public DateTime? Modified { get; set; }
        public ChangeFrequency? ChangeFrequency { get; set; }
        public double? Priority { get; set; }
    }
}

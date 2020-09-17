using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AcBlog.Sdk.Sitemap
{
    public class SitemapBuilder
    {
        private readonly XNamespace _nS = "http://www.sitemaps.org/schemas/sitemap/0.9"!;

        private readonly List<SitemapUrl> _urls;

        public SitemapBuilder()
        {
            _urls = new List<SitemapUrl>();
        }

        public void AddUrl(string url, DateTime? modified = null, ChangeFrequency? changeFrequency = null, double? priority = null)
        {
            _urls.Add(new SitemapUrl()
            {
                Url = url,
                Modified = modified,
                ChangeFrequency = changeFrequency,
                Priority = priority,
            });
        }

        public XDocument Build()
        {
            var sitemap = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(_nS + "urlset",
                    from item in _urls
                    select CreateItemElement(item)
                    ));

            return sitemap;
        }

        private XElement CreateItemElement(SitemapUrl url)
        {
            XElement itemElement = new XElement(_nS + "url", new XElement(_nS + "loc", url.Url.ToLower()));

            if (url.Modified.HasValue)
            {
                itemElement.Add(new XElement(_nS + "lastmod", url.Modified.Value.ToString("yyyy-MM-ddTHH:mm:ss.f") + "+00:00"));
            }

            if (url.ChangeFrequency.HasValue)
            {
                itemElement.Add(new XElement(_nS + "changefreq", url.ChangeFrequency.Value.ToString().ToLower()));
            }

            if (url.Priority.HasValue)
            {
                itemElement.Add(new XElement(_nS + "priority", url.Priority.Value.ToString("N1")));
            }

            return itemElement;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using AcBlog.SDK;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AcBlog.Client.WebAssembly.Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        public enum ChangeFrequency
        {
            Always,
            Hourly,
            Daily,
            Weekly,
            Monthly,
            Yearly,
            Never
        }

        public class SitemapUrl
        {
            public string Url { get; set; }
            public DateTime? Modified { get; set; }
            public ChangeFrequency? ChangeFrequency { get; set; }
            public double? Priority { get; set; }
        }

        public class SitemapBuilder
        {
            private readonly XNamespace NS = "http://www.sitemaps.org/schemas/sitemap/0.9";

            private List<SitemapUrl> _urls;

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

            public override string ToString()
            {
                var sitemap = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement(NS + "urlset",
                        from item in _urls
                        select CreateItemElement(item)
                        ));

                return sitemap.ToString();
            }

            private XElement CreateItemElement(SitemapUrl url)
            {
                XElement itemElement = new XElement(NS + "url", new XElement(NS + "loc", url.Url.ToLower()));

                if (url.Modified.HasValue)
                {
                    itemElement.Add(new XElement(NS + "lastmod", url.Modified.Value.ToString("yyyy-MM-ddTHH:mm:ss.f") + "+00:00"));
                }

                if (url.ChangeFrequency.HasValue)
                {
                    itemElement.Add(new XElement(NS + "changefreq", url.ChangeFrequency.Value.ToString().ToLower()));
                }

                if (url.Priority.HasValue)
                {
                    itemElement.Add(new XElement(NS + "priority", url.Priority.Value.ToString("N1")));
                }

                return itemElement;
            }
        }

        private IBlogService BlogService { get; }

        public IConfiguration Configuration { get; }

        public SiteController(IBlogService blogService, IConfiguration configuration)
        {
            BlogService = blogService;
            Configuration = configuration;
        }

        [HttpGet("sitemap")]
        [HttpGet("sitemap.xml")]
        public async Task<ActionResult> GetSitemap()
        {
            var siteMapBuilder = new SitemapBuilder();

            string baseAddress = Configuration.GetBaseAddress().TrimEnd('/');
            siteMapBuilder.AddUrl(baseAddress);
            {
                var posts = await BlogService.PostService.All();
                siteMapBuilder.AddUrl($"{baseAddress}/posts");
                siteMapBuilder.AddUrl($"{baseAddress}/articles");
                siteMapBuilder.AddUrl($"{baseAddress}/slides");
                siteMapBuilder.AddUrl($"{baseAddress}/notes");
                foreach (var id in posts)
                {
                    siteMapBuilder.AddUrl($"{baseAddress}/posts/{id}");
                }
            };
            {
                var keywords = await BlogService.KeywordService.All();
                siteMapBuilder.AddUrl($"{baseAddress}/keywords");
                foreach (var id in keywords)
                {
                    siteMapBuilder.AddUrl($"{baseAddress}/keywords/{id}");
                }
            };
            {
                var categories = await BlogService.CategoryService.All();
                siteMapBuilder.AddUrl($"{baseAddress}/categories");
                foreach (var id in categories)
                {
                    siteMapBuilder.AddUrl($"{baseAddress}/categories/{id}");
                }
            };
            return Content(siteMapBuilder.ToString(), "text/xml");
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using AcBlog.Client.UI.Models;
using AcBlog.Data.Models;
using AcBlog.Sdk;
using AcBlog.Sdk.Sitemap;
using AcBlog.Sdk.Syndication;
using Markdig;
using Markdig.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AcBlog.Client.WebAssembly.Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {

        private IBlogService BlogService { get; }

        public IConfiguration Configuration { get; }

        private string BaseAddress { get; }

        public SiteController(IBlogService blogService, IConfiguration configuration)
        {
            BlogService = blogService;
            Configuration = configuration;
            BaseAddress = Configuration.GetBaseAddress().TrimEnd('/');
        }

        [HttpGet("sitemap")]
        [HttpGet("sitemap.xml")]
        public async Task<ActionResult> GetSitemap()
        {
            var siteMapBuilder = await BlogService.BuildSitemap(BaseAddress);
            StringBuilder sb = new StringBuilder();
            await using (var writer = XmlWriter.Create(sb))
                siteMapBuilder.Build().WriteTo(writer);
            return Content(sb.ToString(), "text/xml");
        }

        [HttpGet("atom")]
        [HttpGet("atom.xml")]
        public async Task<ActionResult> GetAtomFeed()
        {
            var feed = await BlogService.BuildSyndication(BaseAddress);
            StringBuilder sb = new StringBuilder();
            await using (var writer = XmlWriter.Create(sb))
                feed.GetAtom10Formatter().WriteTo(writer);
            return Content(sb.ToString(), "application/atom+xml");
        }

        [HttpGet("rss")]
        [HttpGet("rss.xml")]
        public async Task<ActionResult> GetRssFeed()
        {
            var feed = await BlogService.BuildSyndication(BaseAddress);
            StringBuilder sb = new StringBuilder();
            await using (var writer = XmlWriter.Create(sb))
                feed.GetRss20Formatter().WriteTo(writer);
            return Content(sb.ToString(), "application/rss+xml");
        }
    }
}
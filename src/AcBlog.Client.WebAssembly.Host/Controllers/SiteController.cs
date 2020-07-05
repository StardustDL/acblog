using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using AcBlog.Client.WebAssembly.Host.Models;
using AcBlog.Client.WebAssembly.Models;
using AcBlog.Data.Models;
using AcBlog.Sdk;
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

        public BlogSettings BlogSettings { get; }

        private string BaseAddress { get; }

        static MarkdownPipeline Pipeline { get; } = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

        public SiteController(IBlogService blogService, IConfiguration configuration, BlogSettings blogSettings)
        {
            BlogService = blogService;
            Configuration = configuration;
            BlogSettings = blogSettings;
            BaseAddress = Configuration.GetBaseAddress().TrimEnd('/');
        }

        [HttpGet("sitemap")]
        [HttpGet("sitemap.xml")]
        public async Task<ActionResult> GetSitemap()
        {
            var siteMapBuilder = new SitemapBuilder();


            siteMapBuilder.AddUrl(BaseAddress);
            {
                var posts = await BlogService.PostService.All();
                siteMapBuilder.AddUrl($"{BaseAddress}/posts");
                siteMapBuilder.AddUrl($"{BaseAddress}/articles");
                siteMapBuilder.AddUrl($"{BaseAddress}/slides");
                siteMapBuilder.AddUrl($"{BaseAddress}/notes");
                foreach (var id in posts)
                {
                    siteMapBuilder.AddUrl($"{BaseAddress}/posts/{id}");
                }
            };
            /*{
                var keywords = await BlogService.KeywordService.All();
                siteMapBuilder.AddUrl($"{BaseAddress}/keywords");
                foreach (var id in keywords)
                {
                    siteMapBuilder.AddUrl($"{BaseAddress}/keywords/{id}");
                }
            };
            {
                var categories = await BlogService.CategoryService.All();
                siteMapBuilder.AddUrl($"{BaseAddress}/categories");
                foreach (var id in categories)
                {
                    siteMapBuilder.AddUrl($"{BaseAddress}/categories/{id}");
                }
            };*/
            return Content(siteMapBuilder.ToString(), "text/xml");
        }

        [HttpGet("atom")]
        [HttpGet("atom.xml")]
        public async Task<ActionResult> GetFeed()
        {
            SyndicationFeed feed = new SyndicationFeed(BlogSettings.Name, BlogSettings.Description, new Uri(BaseAddress));
            SyndicationPerson author = new SyndicationPerson("", BlogSettings.Onwer, BaseAddress);
            feed.Authors.Add(author);
            Dictionary<string, SyndicationCategory> categoryMap = new Dictionary<string, SyndicationCategory>();
            {
                /*var cates = await BlogService.CategoryService.GetCategories(await BlogService.CategoryService.All());
                foreach (var p in cates)
                {
                    var cate = new SyndicationCategory(p.Name);
                    categoryMap.Add(p.Id, cate);
                    feed.Categories.Add(cate);
                }*/
            }
            {
                var posts = await BlogService.PostService.GetPosts(await BlogService.PostService.All());
                List<SyndicationItem> items = new List<SyndicationItem>();
                foreach (var p in posts)
                {
                    var s = new SyndicationItem(p.Title,
                        SyndicationContent.CreateHtmlContent(Markdown.ToHtml(p.Content.Raw, Pipeline)),
                        new Uri($"{BaseAddress}/posts/{p.Id}"), p.Id, p.ModificationTime);
                    s.Authors.Add(author);
                    string summary = Markdown.ToPlainText(p.Content.Raw, Pipeline);
                    s.Summary = SyndicationContent.CreatePlaintextContent(summary.Length <= 100 ? summary : summary.Substring(0, 100));
                    /*if (categoryMap.TryGetValue(p.CategoryId, out var cate))
                        s.Categories.Add(cate);*/
                    s.PublishDate = p.CreationTime;
                    items.Add(s);
                }
                feed.Items = items.AsEnumerable();
            }
            StringBuilder sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
                feed.GetAtom10Formatter().WriteTo(writer);
            return Content(sb.ToString(), "text/xml");
        }
    }
}
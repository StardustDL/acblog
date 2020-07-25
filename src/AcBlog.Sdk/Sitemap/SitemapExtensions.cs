using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Sdk.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;

namespace AcBlog.Sdk.Sitemap
{
    public static class SyndicationExtensions
    {
        public static async Task<SitemapBuilder> BuildSitemap(this IBlogService service, string baseAddress)
        {
            SitemapBuilder siteMapBuilder = new SitemapBuilder();
            ClientUrlGenerator generator = new ClientUrlGenerator
            {
                BaseAddress = baseAddress
            };
            siteMapBuilder.AddUrl(baseAddress);
            {
                siteMapBuilder.AddUrl(generator.Posts());
                siteMapBuilder.AddUrl(generator.Articles());
                siteMapBuilder.AddUrl(generator.Slides());
                siteMapBuilder.AddUrl(generator.Notes());
                siteMapBuilder.AddUrl(generator.Archives());
                {
                    var posts = await service.PostService.GetAllItems();
                    foreach (var c in posts)
                    {
                        if (c is null)
                            continue;
                        siteMapBuilder.AddUrl(generator.Post(c));
                    }
                }
                {
                    siteMapBuilder.AddUrl(generator.Categories());
                    var cates = await service.PostService.GetCategories();
                    foreach (var c in cates.AsCategoryList())
                    {
                        siteMapBuilder.AddUrl(generator.Category(c));
                    }
                }
                {
                    siteMapBuilder.AddUrl(generator.Keywords());
                    var cates = await service.PostService.GetKeywords();
                    foreach (var c in cates.Items)
                    {
                        siteMapBuilder.AddUrl(generator.Keyword(c));
                    }
                }
                {
                    var cates = await service.PageService.GetAllItems();
                    foreach (var c in cates)
                    {
                        if (c is null)
                            continue;
                        siteMapBuilder.AddUrl(generator.Page(c));
                    }
                }
            }

            return siteMapBuilder;
        }
    }
}

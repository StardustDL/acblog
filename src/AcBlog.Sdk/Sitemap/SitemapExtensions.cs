using AcBlog.Data.Extensions;
using AcBlog.Sdk.Helpers;
using System.Threading.Tasks;

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
                siteMapBuilder.AddUrl(generator.Comments());
                {
                    var posts = service.PostService.GetAllItems().IgnoreNull();
                    await foreach (var c in posts)
                    {
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
                    var cates = service.PageService.GetAllItems().IgnoreNull();
                    await foreach (var c in cates)
                    {
                        siteMapBuilder.AddUrl(generator.Page(c));
                    }
                }
            }

            return siteMapBuilder;
        }
    }
}

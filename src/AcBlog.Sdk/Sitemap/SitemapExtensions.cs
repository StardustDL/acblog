using AcBlog.Sdk.Helpers;
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
                var posts = await service.PostService.All();
                foreach (var id in posts)
                {
                    siteMapBuilder.AddUrl(generator.Post(id));
                }
            }

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

            return siteMapBuilder;
        }
    }
}

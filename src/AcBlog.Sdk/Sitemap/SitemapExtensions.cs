using System.Threading.Tasks;
using System.Web;

namespace AcBlog.Sdk.Sitemap
{
    public static class SyndicationExtensions
    {
        public static async Task<SitemapBuilder> BuildSitemap(this IBlogService service, string baseAddress)
        {
            SitemapBuilder siteMapBuilder = new SitemapBuilder();
            siteMapBuilder.AddUrl(baseAddress);
            {
                siteMapBuilder.AddUrl($"{baseAddress}/posts");
                siteMapBuilder.AddUrl($"{baseAddress}/articles");
                siteMapBuilder.AddUrl($"{baseAddress}/slides");
                siteMapBuilder.AddUrl($"{baseAddress}/notes");
                var posts = await service.PostService.All();
                foreach (var id in posts)
                {
                    siteMapBuilder.AddUrl($"{baseAddress}/posts/{HttpUtility.UrlEncode(id)}");
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

using AcBlog.Data.Models.Actions;
using AcBlog.Services.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Services.Extensions
{
    public static class BlogServiceExtensions
    {
        public static Task<QueryResponse<string>> QuerySitemap(this IBlogService service, string baseAddress, CancellationToken cancellationToken = default)
        {
            return service.Query(new Models.BlogQueryRequest
            {
                Type = BlogQueryRequestStrings.Sitemap,
                Data = new System.Collections.Generic.Dictionary<string, string>
                {
                    [BlogQueryRequestStrings.BaseAddress] = baseAddress
                }
            }, cancellationToken);
        }

        public static Task<QueryResponse<string>> QueryaAtomFeed(this IBlogService service, string baseAddress, CancellationToken cancellationToken = default)
        {
            return service.Query(new Models.BlogQueryRequest
            {
                Type = BlogQueryRequestStrings.AtomFeed,
                Data = new System.Collections.Generic.Dictionary<string, string>
                {
                    [BlogQueryRequestStrings.BaseAddress] = baseAddress
                }
            }, cancellationToken);
        }
    }
}

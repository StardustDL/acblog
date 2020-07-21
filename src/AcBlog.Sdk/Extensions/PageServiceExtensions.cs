using AcBlog.Sdk.Filters;

namespace AcBlog.Sdk.Extensions
{
    public static class PageServiceExtensions
    {
        public static PageRouteFilter CreateRouteFilter(this IPageService service)
        {
            return new PageRouteFilter(service);
        }
    }
}

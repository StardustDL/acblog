using AcBlog.Sdk.Filters;

namespace AcBlog.Sdk
{
    public static class PageServiceExtensions
    {
        public static PageRouteFilter CreateRouteFilter(this IPageService service)
        {
            return new PageRouteFilter(service);
        }
    }
}

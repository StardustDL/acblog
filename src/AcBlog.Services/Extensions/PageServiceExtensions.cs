using AcBlog.Services.Filters;

namespace AcBlog.Services.Extensions
{
    public static class PageServiceExtensions
    {
        public static PageRouteFilter CreateRouteFilter(this IPageService service)
        {
            return new PageRouteFilter(service);
        }
    }
}

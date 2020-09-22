using AcBlog.Client.Models;
using AcBlog.Client.UI.Models;
using AcBlog.Extensions;
using AcBlog.UI.Components;
using AcBlog.UI.Components.Loading;
using AcBlog.UI.Components.Slides;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AcBlog.Client.UI
{
    public static class ServiceExtensions
    {
        public static void AddUIComponents(this IServiceCollection services)
        {
            services.AddExtensions()
                .AddExtension<ClientUIComponent>()
                .AddExtension<LoadingUIComponent>()
                .AddExtension<MarkdownUIComponent>()
                .AddExtension<SlidesUIComponent>()
                .AddExtension<AntDesignUIComponent>();
        }
    }
}

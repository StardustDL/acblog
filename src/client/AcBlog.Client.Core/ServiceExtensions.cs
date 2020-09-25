using AcBlog.Client.Models;
using AcBlog.Data.Pages;
using AcBlog.Data.Repositories;
using AcBlog.Sdk;
using AcBlog.Services.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AcBlog.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web.Extensions;
using AcBlog.Services.Generators;

namespace AcBlog.Client
{


    public static class ServiceExtensions
    {
        public static void AddClientConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServerSettings>(configuration.GetSection("Server"));
            services.Configure<BuildStatus>(configuration.GetSection("Build"));
        }

        public static IServiceCollection AddBlogService(this IServiceCollection services, string baseAddress)
        {
            services.PostConfigure<ServerSettings>(settings =>
            {
                settings.BaseAddress = baseAddress;
            });
            services.AddScoped<IBlogService, ClientBlogService>();

            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService);
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PageService);
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().LayoutService);
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().CommentService);
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().StatisticService);
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().FileService);
            services.AddScoped<IPostRepository>(sp => sp.GetRequiredService<IBlogService>().PostService);
            services.AddScoped<IPageRepository>(sp => sp.GetRequiredService<IBlogService>().PageService);
            services.AddScoped<ILayoutRepository>(sp => sp.GetRequiredService<IBlogService>().LayoutService);
            services.AddScoped<ICommentRepository>(sp => sp.GetRequiredService<IBlogService>().CommentService);
            services.AddScoped<IStatisticRepository>(sp => sp.GetRequiredService<IBlogService>().StatisticService);
            services.AddScoped<IFileRepository>(sp => sp.GetRequiredService<IBlogService>().FileService);

            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateArticleFilter());
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateSlidesFilter());
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateNoteFilter());
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateKeywordFilter());
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateCategoryFilter());

            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PageService.CreateRouteFilter());

            services.AddScoped<IMarkdownRenderService, MarkdownRenderService>();
            services.AddScoped<IPageRenderService, PageRenderService>();

            services.AddScoped<IClientUriGenerator, ClientUriGenerator>();

            return services;
        }

        public static IServiceCollection AddBlogServiceAuth(this IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddScoped<AccessTokenProvider>();
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            return services;
        }
    }
}

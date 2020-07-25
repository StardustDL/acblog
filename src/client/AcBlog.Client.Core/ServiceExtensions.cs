using AcBlog.Client.Models;
using AcBlog.Sdk;
using AcBlog.Sdk.Api;
using AcBlog.Sdk.FileSystem;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using StardustDL.Extensions.FileProviders.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using AcBlog.Sdk.Extensions;
using AcBlog.Data.Pages;
using AcBlog.Data.Repositories;

namespace AcBlog.Client
{
    public static class ServiceExtensions
    {
        public static void AddClientConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServerSettings>(configuration.GetSection("Server"));
            services.Configure<BuildStatus>(configuration.GetSection("Build"));
        }

        public static void AddBlogService(this IServiceCollection services, string baseAddress)
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
        }
    }
}

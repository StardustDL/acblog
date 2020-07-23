using AcBlog.Client.WebAssembly.Models;
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

namespace AcBlog.Client.WebAssembly
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

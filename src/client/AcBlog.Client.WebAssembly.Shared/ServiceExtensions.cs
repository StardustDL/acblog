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

namespace AcBlog.Client.WebAssembly
{
    public static class ServiceExtensions
    {
        public static void AddClientConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BuildStatus>(configuration.GetSection("Server"));
            services.Configure<IdentityProvider>(configuration.GetSection("IdentityProvider"));
            services.Configure<BuildStatus>(configuration.GetSection("Build"));
        }

        public static void AddBlogService(this IServiceCollection services, string baseAddress)
        {
            services.AddScoped<IBlogService>(sp =>
            {
                var server = sp.GetRequiredService<IOptions<ServerSettings>>().Value;
                var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };
                if (string.IsNullOrEmpty(server.Url))
                {
                    client.BaseAddress = new Uri($"{baseAddress.TrimEnd('/')}/data/");
                    return new FileSystemBlogService(new HttpFileProvider(client));
                }
                else if (server.IsStatic)
                {
                    if (!server.Url.EndsWith("/"))
                        server.Url += "/";
                    client.BaseAddress = new Uri(server.Url);
                    return new FileSystemBlogService(new HttpFileProvider(client));
                }
                else
                {
                    client.BaseAddress = new Uri(server.Url);
                    return new ApiBlogService(client);
                }
            });

            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateArticleFilter());
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateSlidesFilter());
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateNoteFilter());
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateKeywordFilter());
            services.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateCategoryFilter());
        }
    }
}

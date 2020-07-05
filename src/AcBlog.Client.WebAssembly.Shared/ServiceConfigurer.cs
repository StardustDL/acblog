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

namespace AcBlog.Client.WebAssembly
{
    public static class ServiceConfigurer
    {
        public static void AddBlogService(this IServiceCollection serviceCollection, ServerSettings server, string baseAddress)
        {
            if (string.IsNullOrEmpty(server.Url))
            {
                serviceCollection.AddHttpClient("static-file-provider", client => client.BaseAddress = new Uri($"{baseAddress.TrimEnd('/')}/data/"));
                serviceCollection.AddScoped<IBlogService>(sp =>
                {
                    return new FileSystemBlogService(new HttpFileProvider(
                        sp.GetRequiredService<IHttpClientFactory>().CreateClient("static-file-provider")));
                });
            }
            else if (server.IsStatic)
            {
                if (!server.Url.EndsWith("/"))
                    server.Url += "/";
                var uri = new Uri(server.Url);
                serviceCollection.AddHttpClient("static-file-provider", client => client.BaseAddress = uri);
                serviceCollection.AddScoped<IBlogService>(sp =>
                {
                    return new FileSystemBlogService(new HttpFileProvider(
                        sp.GetRequiredService<IHttpClientFactory>().CreateClient("static-file-provider")));
                });
            }
            else
            {
                var uri = new Uri(server.Url);
                serviceCollection.AddHttpClient("api-provider", client => client.BaseAddress = uri);
                serviceCollection.AddScoped<IBlogService>(sp =>
                {
                    return new ApiBlogService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("api-provider"));
                });
            }
            serviceCollection.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateArticleFilter());
            serviceCollection.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateSlidesFilter());
            serviceCollection.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateNoteFilter());
            serviceCollection.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateKeywordFilter());
            serviceCollection.AddScoped(sp => sp.GetRequiredService<IBlogService>().PostService.CreateCategoryFilter());
        }
    }
}

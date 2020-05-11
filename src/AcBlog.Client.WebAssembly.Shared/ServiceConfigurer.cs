using AcBlog.Client.WebAssembly.Models;
using AcBlog.SDK;
using AcBlog.SDK.API;
using AcBlog.SDK.StaticFile;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace AcBlog.Client.WebAssembly
{
    public static class ServiceConfigurer
    {
        public static void AddBlogService(this IServiceCollection serviceCollection, ServerSettings server, string baseAddress)
        {
            if (string.IsNullOrEmpty(server.Url))
            {
                serviceCollection.AddHttpClient("static-file-provider", client => client.BaseAddress = new Uri(baseAddress));
                serviceCollection.AddScoped<IBlogService>(sp =>
                {
                    return new HttpStaticFileBlogService("/data",
                        sp.GetRequiredService<IHttpClientFactory>().CreateClient("static-file-provider"));
                });
            }
            else if (server.IsStatic)
            {
                var uri = new Uri(server.Url);
                serviceCollection.AddHttpClient("static-file-provider", client => client.BaseAddress = uri);
                serviceCollection.AddScoped<IBlogService>(sp =>
                {
                    return new HttpStaticFileBlogService(uri.LocalPath,
                        sp.GetRequiredService<IHttpClientFactory>().CreateClient("static-file-provider"));
                });
            }
            else
            {
                var uri = new Uri(server.Url);
                serviceCollection.AddHttpClient("api-provider", client => client.BaseAddress = uri);
                serviceCollection.AddScoped<IBlogService>(sp =>
                {
                    return new HttpApiBlogService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("api-provider"));
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

using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AcBlog.SDK;
using AcBlog.SDK.API;
using AcBlog.Client.WebAssembly.Models;
using AcBlog.SDK.StaticFile;
using System.IO;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration.Json;
using AcBlog.SDK.Filters;

namespace AcBlog.Client.WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            {
                using var client = new HttpClient()
                {
                    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                };
                using var response = await client.GetAsync("build.json");
                response.EnsureSuccessStatusCode();
                using var stream = await response.Content.ReadAsStreamAsync();

                builder.Configuration.AddJsonStream(stream);
            }

            builder.RootComponents.Add<App>("app");

            string server = builder.Configuration.GetValue<string>("APIServer", null);

            var blogSettings = new BlogSettings()
            {
                Name = "AcBlog",
                Description = "A blog system based on WebAssembly.",
                IndexIconUrl = "icon.png",
                Footer = "",
                StartYear = DateTimeOffset.Now.Year,
                IsStaticServer = true
            };
            builder.Configuration.Bind("BlogSettings", blogSettings);
            builder.Services.AddSingleton(blogSettings);

            if (string.IsNullOrEmpty(server))
            {
                builder.Services.AddHttpClient("static-file-provider", client => client.BaseAddress = new Uri(Path.Join(builder.HostEnvironment.BaseAddress, "data")));
                builder.Services.AddSingleton<IBlogService>(sp =>
                {
                    return new HttpStaticFileBlogService("/data",
                        sp.GetRequiredService<IHttpClientFactory>().CreateClient("static-file-provider"));
                });
            }
            else if (blogSettings.IsStaticServer)
            {
                var uri = new Uri(server);
                builder.Services.AddHttpClient("static-file-provider", client => client.BaseAddress = uri);
                builder.Services.AddSingleton<IBlogService>(sp =>
                {
                    return new HttpStaticFileBlogService(uri.LocalPath,
                        sp.GetRequiredService<IHttpClientFactory>().CreateClient("static-file-provider"));
                });
            }
            else
            {
                builder.Services.AddHttpClient<IBlogService, HttpApiBlogService>(
                    client => client.BaseAddress = new Uri(server));
            }
            builder.Services.AddSingleton(sp => sp.GetRequiredService<IBlogService>().PostService.CreateArticleFilter());
            builder.Services.AddSingleton(sp => sp.GetRequiredService<IBlogService>().PostService.CreateSlidesFilter());

            await builder.Build().RunAsync();
        }
    }
}

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
using System.Net.Http.Json;

namespace AcBlog.Client.WebAssembly
{
    public class Program
    {
        public static bool HasHost { get; set; } = false;

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Logging.SetMinimumLevel(LogLevel.Warning);


            {
                using var client = new HttpClient()
                {
                    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                };
                {
                    using var response = await client.GetAsync("Server/Test");
                    HasHost = response.IsSuccessStatusCode;
                }

                var build = await LoadBuildStatus(builder, client);
                var server = await LoadServerSettings(builder, client);
                var blog = await LoadBlogSettings(builder, client);

                builder.Services.AddSingleton(build);
                builder.Services.AddSingleton(server);
                builder.Services.AddSingleton(blog);

                builder.Services.AddBlogService(server, builder.HostEnvironment.BaseAddress);
            }

            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }

        static async Task<ServerSettings> LoadServerSettings(WebAssemblyHostBuilder builder, HttpClient client)
        {
            if (HasHost)
            {
                using var response = await client.GetAsync("Server/Server");
                return await response.Content.ReadFromJsonAsync<ServerSettings>();
            }
            else
            {
                ServerSettings server = new ServerSettings();
                builder.Configuration.Bind("Server", server);
                return server;
            }
        }

        static async Task<BuildStatus> LoadBuildStatus(WebAssemblyHostBuilder builder, HttpClient client)
        {
            if (HasHost)
            {
                using var response = await client.GetAsync("/Server/Build");
                return await response.Content.ReadFromJsonAsync<BuildStatus>();
            }
            else
            {
                using var response2 = await client.GetAsync("build.json");
                response2.EnsureSuccessStatusCode();
                using var stream = await response2.Content.ReadAsStreamAsync();
                builder.Configuration.AddJsonStream(stream);

                BuildStatus server = new BuildStatus();
                builder.Configuration.Bind("Build", server);
                return server;
            }
        }

        static async Task<BlogSettings> LoadBlogSettings(WebAssemblyHostBuilder builder, HttpClient client)
        {
            if (HasHost)
            {
                using var response = await client.GetAsync("/Server/Blog");
                return await response.Content.ReadFromJsonAsync<BlogSettings>();
            }
            else
            {
                var blogSettings = new BlogSettings()
                {
                    Name = "AcBlog",
                    Description = "A blog system based on WebAssembly.",
                    IndexIconUrl = "icon.png",
                    Footer = "",
                    StartYear = DateTimeOffset.Now.Year,
                    IsStaticServer = true
                };
                builder.Configuration.Bind("Blog", blogSettings);
                return blogSettings;
            }
        }
    }
}

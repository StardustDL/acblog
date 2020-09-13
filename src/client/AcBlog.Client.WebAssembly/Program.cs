using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AcBlog.Sdk;
using AcBlog.Sdk.Api;
using AcBlog.Client.UI.Models;
using AcBlog.Sdk.FileSystem;
using System.IO;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration.Json;
using AcBlog.Sdk.Filters;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using AcBlog.UI.Components;
using AcBlog.Extensions;
using Microsoft.Extensions.Options;
using AcBlog.Client.Models;
using AcBlog.Client.UI;

namespace AcBlog.Client.WebAssembly
{
    public class Program
    {
        public static bool HasHost { get; set; }

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            if (builder.HostEnvironment.IsProduction())
            {
                builder.Logging.SetMinimumLevel(LogLevel.Warning);
            }

            builder.Services.AddSingleton(new RuntimeOptions { IsPrerender = false });

            builder.Services.AddUIComponents();

            var client = new HttpClient()
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            };
            builder.Services.AddSingleton(client);

            builder.Services.AddHttpClient();

            {
                {
                    try
                    {
                        using var response = await client.GetAsync("Server/Test");
                        response.EnsureSuccessStatusCode();
                        HasHost = await response.Content.ReadFromJsonAsync<bool>();
                    }
                    catch
                    {
                        HasHost = false;
                    }
                }

                await LoadBuildStatus(builder, client);
                var serverSettings = await LoadServerSettings(builder, client);

                builder.Services.AddClientConfigurations(builder.Configuration);
                builder.Services.AddClientAuthorization(serverSettings.Identity);
                builder.Services.AddBlogService(builder.HostEnvironment.BaseAddress);
            }

            builder.RootComponents.Add<AcBlog.Client.UI.App>("app");

            await builder.UseExtensions();

            await builder.Build().RunAsync();
        }

        static async Task<ServerSettings> LoadServerSettings(WebAssemblyHostBuilder builder, HttpClient client)
        {
            if (HasHost)
            {
                using var response = await client.GetAsync("Server/Server");
                await using var stream = await response.Content.ReadAsStreamAsync();
                builder.Configuration.AddJsonStream(stream);
            }
            else
            {
            }
            ServerSettings res = new ServerSettings();
            builder.Configuration.GetSection("Server").Bind(res);
            return res;
        }

        static async Task<BuildStatus> LoadBuildStatus(WebAssemblyHostBuilder builder, HttpClient client)
        {
            if (HasHost)
            {
                using var response = await client.GetAsync("/Server/Build");
                await using var stream = await response.Content.ReadAsStreamAsync();
                builder.Configuration.AddJsonStream(stream);
            }
            else
            {
                using var response = await client.GetAsync("build.json");
                response.EnsureSuccessStatusCode();
                await using var stream = await response.Content.ReadAsStreamAsync();
                builder.Configuration.AddJsonStream(stream);
            }
            BuildStatus res = new BuildStatus();
            builder.Configuration.GetSection("Build").Bind(res);
            return res;
        }
    }
}

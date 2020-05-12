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
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace AcBlog.Client.WebAssembly
{
    public class Program
    {
        public static bool HasHost { get; set; } = false;

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            if (builder.HostEnvironment.IsProduction())
            {
                builder.Logging.SetMinimumLevel(LogLevel.Warning);
            }

            {
                using var client = new HttpClient()
                {
                    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                };
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

                var build = await LoadBuildStatus(builder, client);
                var server = await LoadServerSettings(builder, client);
                var blog = await LoadBlogSettings(builder, client);
                var identityProvider = await LoadIdentityProvider(builder, client);

                builder.Services.AddSingleton(build);
                builder.Services.AddSingleton(server);
                builder.Services.AddSingleton(blog);
                builder.Services.AddSingleton(identityProvider);

                if (identityProvider.Enable)
                {
                    builder.Services.AddApiAuthorization(options =>
                    {
                        options.ProviderOptions.ConfigurationEndpoint = identityProvider.Endpoint;
                        options.AuthenticationPaths.RemoteProfilePath = identityProvider.RemoteProfilePath;
                        options.AuthenticationPaths.RemoteRegisterPath = identityProvider.RemoteRegisterPath;
                    });
                }
                else
                {
                    builder.Services.AddAuthorizationCore();
                    builder.Services.AddSingleton<AuthenticationStateProvider, EmptyAuthenticationStateProvider>();
                    builder.Services.AddSingleton<SignOutSessionStateManager>();
                }

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

        static async Task<IdentityProvider> LoadIdentityProvider(WebAssemblyHostBuilder builder, HttpClient client)
        {
            if (HasHost)
            {
                using var response = await client.GetAsync("Server/Identity");
                return await response.Content.ReadFromJsonAsync<IdentityProvider>();
            }
            else
            {
                IdentityProvider server = new IdentityProvider();
                builder.Configuration.Bind("IdentityProvider", server);
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
                var blogSettings = new BlogSettings();
                builder.Configuration.Bind("Blog", blogSettings);
                return blogSettings;
            }
        }
    }
}

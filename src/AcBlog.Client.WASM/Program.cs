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
using AcBlog.Client.WASM.Models;

namespace AcBlog.Client.WASM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            string server = builder.Configuration.GetValue<string>("APIServer", null);

            {
                var blogSettings = new BlogSettings()
                {
                    Name = "AcBlog",
                    Description = "A blog system based on WebAssembly.",
                    IconUrl = "icon-512.png"
                };
                builder.Configuration.Bind("BlogSettings", blogSettings);

                builder.Services.AddSingleton(blogSettings);
            }

            if (server == null)
            {
                builder.Services.AddHttpClient<IBlogService, HttpApiBlogService>(
                    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            }
            else
            {
                builder.Services.AddHttpClient<IBlogService, HttpApiBlogService>(
                    client => client.BaseAddress = new Uri(server));
            }

            await builder.Build().RunAsync();
        }
    }
}

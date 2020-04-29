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

namespace AcBlog.Client.WASM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddHttpClient<IBlogService, HttpApiBlogService>(
                client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("APIServer")));

            await builder.Build().RunAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AcBlog.Client.WebAssembly.Models;
using AcBlog.SDK;
using AcBlog.SDK.StaticFile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AcBlog.Client.WebAssembly.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddControllers();

            services.AddHttpClient();

            ServerSettings server = new ServerSettings();
            Configuration.Bind("Server", server);
            services.AddSingleton(server);

            {
                BuildStatus bs = new BuildStatus();
                Configuration.Bind("build", bs);
                services.AddSingleton(bs);
            }

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
                Configuration.Bind("Blog", blogSettings);
                services.AddSingleton(blogSettings);
            }

            services.AddBlogService(server, Configuration.GetValue<string>("BaseAddress"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

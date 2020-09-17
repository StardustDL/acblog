using AcBlog.Client.Models;
using AcBlog.Client.Server.Areas.Identity;
using AcBlog.Client.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AcBlog.Client.Server
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
            services.AddServerSideBlazor();

            services.AddSingleton(new RuntimeOptions { IsPrerender = false });
            services.AddUIComponents();

            services.AddHttpClient();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddClientConfigurations(Configuration);
            {
                var server = new ServerSettings();
                Configuration.GetSection("Server").Bind(server);

                static void AddServerAuthorization(IServiceCollection services, IdentityServerSettings identityProvider)
                {
                    if (identityProvider.Enable)
                    {
                        services.AddAuthentication();

                        services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
                    }
                    else
                    {
                        services.AddAuthorizationCore();
                        services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
                        services.AddScoped<SignOutSessionStateManager>();
                    }
                }

                AddServerAuthorization(services, server.Identity);
            }

            services.AddBlogService(Configuration.GetBaseAddress());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

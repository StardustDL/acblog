using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AcBlog.Data.Models;
using AcBlog.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using AcBlog.Server.Api.Data;
using AcBlog.Server.Api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpOverrides;
using AcBlog.Data.Repositories.SqlServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AcBlog.Server.Api
{
    public class Startup
    {
        const string DefaultCorsPolicy = nameof(DefaultCorsPolicy);

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            {
                Options options = new Options();
                Configuration.Bind("Options", options);
                services.AddSingleton(options);
            }

            {
                /*services.AddScoped(sp => new DbContextOptionsBuilder<DataContext>()
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                services.AddScoped(sp => sp.GetRequiredService<DbContextOptionsBuilder<DataContext>>().Options);*/

                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

                services.AddScoped<IPostRepository, AcBlog.Data.Repositories.SqlServer.PostRepository>();
            }
            {
                services.AddDbContext<IdentityDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<IdentityDbContext>();

                services.AddIdentityServer(options =>
                {
                    options.PublicOrigin = Configuration.GetValue<string>("BaseAddress");
                })
                    .AddApiAuthorization<ApplicationUser, IdentityDbContext>();

                services.AddAuthentication()
                    .AddIdentityServerJwt();
            }

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AcBlog API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicy,
                    builder =>
                    {
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.WithOrigins(Configuration.GetSection("Cors").Get<string[]>());
                    });
            });
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
                app.UseDatabaseErrorPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(DefaultCorsPolicy);

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AcBlog API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}

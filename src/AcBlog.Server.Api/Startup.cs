using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.SqlServer.Models;
using AcBlog.Server.Api.Data;
using AcBlog.Server.Api.Models;
using AcBlog.Services;
using Hangfire;
using Hangfire.SqlServer;
using Loment;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace AcBlog.Server.Api
{
    public class Startup
    {
        internal string DefaultCorsPolicy { get; } = nameof(DefaultCorsPolicy);

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            {
                services.Configure<AppOptions>(Configuration.GetSection("Options"));
            }

            {
                services.AddDbContext<BlogDataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

                services.AddScoped<IBlogService, AcBlog.Services.SqlServer.SqlServerBlogService>();

                // services.AddScoped<IPostRepository, AcBlog.Data.Repositories.SqlServer.PostRepository>();
            }
            {
                services.AddDbContext<ServiceDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ServiceConnection")));

                // services.AddScoped<IPostRepository, AcBlog.Data.Repositories.SqlServer.PostRepository>();
            }
            {
                services.AddDbContext<IdentityDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<IdentityDbContext>();

                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // avoid ms-default mapping, it will change sub claim's type name

                services.AddIdentityServer(Configuration.GetSection("IdentityServer:Options"))
                    .AddApiAuthorization<ApplicationUser, IdentityDbContext>();

                services.AddAuthentication()
                    .AddIdentityServerJwt();
            }

            services.AddHttpClient();

            services.AddDatabaseDeveloperPageExceptionFilter();

            /*{
                services.Configure<LomentServerOptions>(Configuration.GetSection("LomentServer"));
                services.AddHttpClient("loment-client", (sp, client) =>
                {
                    var options = sp.GetService<IOptions<LomentServerOptions>>().Value;
                    if (options.Enable)
                        client.BaseAddress = new Uri(options.Uri);
                });
                services.AddScoped<ILomentService>(sp =>
                {
                    var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient("loment-client");
                    return new LomentService(http);
                });
            }*/

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AcBlog API", Version = "v1" });

                var apiKey = new OpenApiSecurityScheme
                {
                    Description = "JWT token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };

                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                {
                    Description =
                "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });

                var oar = new OpenApiSecurityRequirement();
                oar.Add(
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    }, new List<string>());
                c.AddSecurityRequirement(oar);
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

            {
                services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(Configuration.GetConnectionString("ServiceConnection"), new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true,
                    }));

                // Add the processing server as IHostedService
                services.AddHangfireServer();
            }
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
                app.UseMigrationsEndPoint();

                app.UseHangfireDashboard();
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AcBlog API V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(DefaultCorsPolicy);

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}

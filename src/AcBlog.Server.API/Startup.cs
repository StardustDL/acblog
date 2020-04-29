using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AcBlog.Data.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace AcBlog.Server.API
{
    public class Startup
    {
        const string _devCorsPolicy = nameof(_devCorsPolicy);

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            {
                string rootPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data");
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }

                string userRootPath = Path.Join(rootPath, "users");
                if (!Directory.Exists(userRootPath))
                {
                    Directory.CreateDirectory(userRootPath);
                }
                string postRootPath = Path.Join(rootPath, "posts");
                if (!Directory.Exists(postRootPath))
                {
                    Directory.CreateDirectory(postRootPath);
                }

                services.AddSingleton<IUserProvider>(sv => new Data.Providers.FileSystem.UserProvider(userRootPath));
                services.AddSingleton<IPostProvider>(sv => new Data.Providers.FileSystem.PostProvider(postRootPath));
            }

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AcBlog API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(_devCorsPolicy,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:5001", "http://localhost:5000");
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseCors(_devCorsPolicy);
            }

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AcBlog API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

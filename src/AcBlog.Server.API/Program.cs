using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AcBlog.Data.Models;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories.FileSystem;
using AcBlog.Server.API.Data;
using AcBlog.Server.API.Models;
using IdentityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AcBlog.Server.API
{
    public static class SeedData
    {
        public static async Task InitializeIdentityDb(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<Data.IdentityDbContext>();
            await context.Database.EnsureCreatedAsync();
            if(!await context.Users.AnyAsync())
            {
                var userStore = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var user = new ApplicationUser
                {
                    UserName = "admin@acblog",
                    Email = "admin@acblog",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                var result = await userStore.CreateAsync(user, "123P$d");

                if (!result.Succeeded)
                {
                    throw new Exception("Create default user failed.");
                }
            }

            await context.SaveChangesAsync();
        }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    await SeedData.InitializeIdentityDb(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            await host.RunAsync();
        }

        static async Task SeedDataForFS(string rootPath)
        {
            /*string userRootPath = Path.Join(rootPath, "users");
            string postRootPath = Path.Join(rootPath, "posts");
            if (!Directory.Exists(userRootPath))
            {
                Directory.CreateDirectory(userRootPath);
            }
            if (!Directory.Exists(postRootPath))
            {
                Directory.CreateDirectory(postRootPath);
            }

            await UserRepositoryBuilder.Build(new[]
            {
                new User
                {
                    Nickname = "admin",
                    Id = Guid.NewGuid().ToString(),
                }
            }, userRootPath, 10);

            await PostRepositoryBuilder.Build(new[]{ new PostBuildData(new Post
            {
                Title = "title",
                Id = Guid.NewGuid().ToString(),
            }) }, new PostProtector(), new DirectoryInfo(postRootPath), 10);*/
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AcBlog.Data.Models;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories.FileSystem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AcBlog.Server.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            string rootPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data");
            await SeedDataForFS(rootPath);

            await host.RunAsync();
        }

        static async Task SeedDataForFS(string rootPath)
        {
            string userRootPath = Path.Join(rootPath, "users");
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

            await PostRepositoryBuilder.Build(new (Post,ProtectionKey)[]
            {
                (new Post
                {
                    Title = "title",
                    Id = Guid.NewGuid().ToString(),
                }, null)
            }, new PostProtector(), postRootPath, 10);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

using AcBlog.Data.Repositories.SqlServer.Models;
using AcBlog.Server.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AcBlog.Server.Api
{
    public static class SeedData
    {
        public static async Task InitializeIdentityDb(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<Data.IdentityDbContext>();
            await context.Database.EnsureCreatedAsync();
            if (!await context.Users.AnyAsync())
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

        public static async Task InitializeDb(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<BlogDataContext>();
            await context.Database.EnsureCreatedAsync();
            await context.SaveChangesAsync();
        }
    }
}

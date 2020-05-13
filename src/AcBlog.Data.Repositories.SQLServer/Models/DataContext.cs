using AcBlog.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Data.Repositories.SQLServer.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<PostData> Posts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Keyword> Keywords { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace AcBlog.Data.Repositories.SqlServer.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<RawPost> Posts { get; set; }
    }
}

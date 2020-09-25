using Microsoft.EntityFrameworkCore;

namespace AcBlog.Server.Api.Data
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(
            DbContextOptions<ServiceDbContext> options) : base(options)
        {
        }
    }
}

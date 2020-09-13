using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AcBlog.Server.Api.Areas.Identity.IdentityHostingStartup))]
namespace AcBlog.Server.Api.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
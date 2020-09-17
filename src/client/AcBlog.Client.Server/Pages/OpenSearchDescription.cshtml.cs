using AcBlog.Data.Models;
using AcBlog.Sdk;
using AcBlog.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AcBlog.Client.Server.Pages
{
    public class OpenSearchDescriptionModel : PageModel
    {
        public OpenSearchDescriptionModel(IBlogService service, IConfiguration configuration)
        {
            Service = service;
            Configuration = configuration;
            BaseAddress = Configuration.GetBaseAddress().TrimEnd('/');
        }

        public IBlogService Service { get; }
        public IConfiguration Configuration { get; }
        public string BaseAddress { get; }
        public BlogOptions BlogOptions { get; set; }

        public async Task OnGetAsync()
        {
            BlogOptions = await Service.GetOptions();
        }
    }
}
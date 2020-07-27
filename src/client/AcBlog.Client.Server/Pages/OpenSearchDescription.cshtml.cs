using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcBlog.Client.UI.Pages.Categories;
using AcBlog.Data.Models;
using AcBlog.Sdk;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

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
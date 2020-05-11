using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcBlog.Client.WebAssembly.Models;
using AcBlog.SDK;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcBlog.Client.WebAssembly.Host.Pages
{
    public class HostModel : PageModel
    {
        public HostModel(BlogSettings blogSettings, IBlogService blogService)
        {
            BlogSettings = blogSettings;
            BlogService = blogService;
        }

        public string Title { get; private set; } = "Loading...";

        private BlogSettings BlogSettings { get; }

        private IBlogService BlogService { get; }

        public async Task OnGetAsync()
        {
            Title = HttpContext.Request.Path;
        }
    }
}
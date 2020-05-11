using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AcBlog.Client.WebAssembly.Models;
using AcBlog.Data.Models;
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

        private Task GeneratePageTitle(string[] path, List<string> result)
        {
            int pageNumber = int.Parse(path[0]);
            result.Add($"Page {pageNumber + 1}");
            return Task.CompletedTask;
        }

        private async Task GeneratePostTitle(string[] path, List<string> result, string header)
        {
            result.Add(header);
            if (path.Length > 0)
            {
                if (path[0] == "pages")
                {
                    await GeneratePageTitle(path[1..], result);
                }
                else
                {
                    string id = path[0];
                    Post value = await BlogService.PostService.Get(id);
                    result.Add(value.Title);
                }
            }
        }

        private async Task GenerateCategoryTitle(string[] path, List<string> result)
        {
            result.Add("Categories");
            if (path.Length > 0)
            {
                if (path[0] == "pages")
                {
                    await GeneratePageTitle(path[1..], result);
                }
                else
                {
                    string id = path[0];
                    Category value = await BlogService.CategoryService.Get(id);
                    result.Add(value.Name);
                    if (path.Length > 1)
                        await GeneratePageTitle(path[2..], result);
                }
            }
        }

        private async Task GenerateKeywordTitle(string[] path, List<string> result)
        {
            result.Add("Keywords");
            if (path.Length > 0)
            {
                if (path[0] == "pages")
                {
                    await GeneratePageTitle(path[1..], result);
                }
                else
                {
                    string id = path[0];
                    Keyword value = await BlogService.KeywordService.Get(id);
                    result.Add(value.Name);
                    if (path.Length > 1)
                        await GeneratePageTitle(path[2..], result);
                }
            }
        }

        public async Task OnGetAsync()
        {
            string path = HttpContext.Request.Path.ToString().Trim('/');
            {
                int indFrag = path.IndexOf('#');
                if (indFrag != -1)
                    path = path.Remove(indFrag);
            }
            string[] pathSegs = path.Split('/');

            List<string> result = new List<string>
            {
                BlogSettings.Name
            };

            if (pathSegs.Length > 0)
            {
                switch (pathSegs[0])
                {
                    case "posts":
                        await GeneratePostTitle(pathSegs[1..], result, "Posts");
                        break;
                    case "articles":
                        await GeneratePostTitle(pathSegs[1..], result, "Articles");
                        break;
                    case "slides":
                        await GeneratePostTitle(pathSegs[1..], result, "Slides");
                        break;
                    case "notes":
                        await GeneratePostTitle(pathSegs[1..], result, "Notes");
                        break;
                    case "categories":
                        await GenerateCategoryTitle(pathSegs[1..], result);
                        break;
                    case "keywords":
                        await GenerateKeywordTitle(pathSegs[1..], result);
                        break;
                }
            }

            result.Reverse();

            Title = string.Join(" - ", result);
        }
    }
}
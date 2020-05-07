using AcBlog.Client.WebAssembly.Interops;
using AcBlog.Client.WebAssembly.Models;
using AcBlog.SDK;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using AcBlog.Client.WebAssembly.Shared;

namespace AcBlog.Client.WebAssembly.Pages.Articles
{
    public class BaseArticlePage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                if (string.IsNullOrEmpty(value))
                    value = $"Articles";
                else
                    value = $"{value} - Articles";
                base.Title = value;
            }
        }
    }
}

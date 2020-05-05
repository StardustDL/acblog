using AcBlog.Client.WASM.Interops;
using AcBlog.Client.WASM.Models;
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
using AcBlog.Client.WASM.Shared;

namespace AcBlog.Client.WASM.Pages.Articles
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

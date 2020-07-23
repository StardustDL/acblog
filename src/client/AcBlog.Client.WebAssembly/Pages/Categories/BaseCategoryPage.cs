using AcBlog.Client.UI.Interops;
using AcBlog.Client.UI.Models;
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

namespace AcBlog.Client.WebAssembly.Pages.Categories
{
    public class BaseCategoryPage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                if (string.IsNullOrEmpty(value))
                    value = $"Categories";
                else
                    value = $"{value} - Categories";
                base.Title = value;
            }
        }
    }
}

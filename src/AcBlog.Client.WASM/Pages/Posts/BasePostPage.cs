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

namespace AcBlog.Client.WASM.Pages.Posts
{
    public class BasePostPage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                if (string.IsNullOrEmpty(value))
                    value = $"Posts";
                else
                    value = $"{value} - Posts";
                base.Title = value;
            }
        }
    }
}

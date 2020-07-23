using AcBlog.Client.UI.Interops;
using AcBlog.Client.UI.Models;
using AcBlog.Sdk;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using AcBlog.Client.UI.Shared;

namespace AcBlog.Client.UI.Pages.Pages
{
    public class BasePagePage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                value = string.IsNullOrEmpty(value) ? $"Pages" : $"{value} - Pages";
                base.Title = value;
            }
        }
    }
}

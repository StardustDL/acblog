using AcBlog.Client.UI.Interops;
using AcBlog.Data.Models;
using AcBlog.Sdk;
using AcBlog.Services;
using AcBlog.Services.Generators;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace AcBlog.Client.UI.Shared
{
    public class BasePage : ComponentBase, IDisposable
    {
        private string _title;

        protected string BaseUri { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IBlogService Service { get; set; }

        [Inject]
        protected IClientUriGenerator UrlGenerator { get; set; }

        protected BlogOptions BlogOptions { get; set; } = new BlogOptions();

        protected virtual string Title
        {
            get
            {
                string title = BlogOptions.Name;
                if (!string.IsNullOrWhiteSpace(_title))
                    title = $"{_title} - {BlogOptions.Name}";
                return title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                }
            }
        }

        private string LocalAnchorJump { get; set; } = "";

        private string GetBaseUri()
        {
            var url = NavigationManager.Uri;
            var ind = url.IndexOf('#');
            if (ind >= 0)
                url = url.Remove(ind);
            return url;
        }

        protected override void OnInitialized()
        {
            BaseUri = GetBaseUri();
            Title = "";
            NavigationManager.LocationChanged += LocationChanged;
            LocationChanged(null, null);
        }

        protected override async Task OnInitializedAsync()
        {
            BlogOptions = await Service.GetOptions();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            /*string title = BlogOptions.Name;
            if (!string.IsNullOrEmpty(Title))
                title = $"{Title} - {BlogOptions.Name}";
            await WindowInterop.SetTitle(JSRuntime, title);*/
            if (!string.IsNullOrEmpty(LocalAnchorJump))
            {
                await WindowInterop.ScrollTo(JSRuntime, LocalAnchorJump);
                LocalAnchorJump = null;
            }
        }

        private void LocationChanged(object sender, LocationChangedEventArgs args)
        {
            var url = NavigationManager.Uri;
            if (url.StartsWith(BaseUri))
            {
                var frag = url[BaseUri.Length..];
                if (frag.StartsWith("#"))
                {
                    LocalAnchorJump = Uri.UnescapeDataString(frag[1..]);
                    StateHasChanged();
                }
            }
        }

        public virtual void Dispose()
        {
            NavigationManager.LocationChanged -= LocationChanged;
            GC.SuppressFinalize(this);
        }
    }
}

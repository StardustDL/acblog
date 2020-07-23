using Microsoft.Extensions.DependencyInjection;
using AcBlog.UI.Components.Loading;
using AcBlog.UI.Components.Slides;
using AcBlog.Extensions;
using AcBlog.Client.UI.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using AcBlog.UI.Components;
using AcBlog.Client.Models;
using AcBlog.Client.UI;

namespace AcBlog.Client.WebAssembly
{
    public static class ServiceExtensions
    {
        public static void AddUIComponents(this IServiceCollection services)
        {
            services.AddExtensions()
                .AddExtension<ClientUIComponent>()
                .AddExtension<LoadingUIComponent>()
                .AddExtension<MarkdownUIComponent>()
                .AddExtension<SlidesUIComponent>()
                .AddExtension<AntDesignUIComponent>();
        }

        public static void AddClientAuthorization(this IServiceCollection services, IdentityServerSettings identityProvider)
        {
            if (identityProvider.Enable)
            {
                services.AddApiAuthorization(options =>
                {
                    options.ProviderOptions.ConfigurationEndpoint = identityProvider.Endpoint;
                    options.AuthenticationPaths.RemoteProfilePath = identityProvider.RemoteProfilePath;
                    options.AuthenticationPaths.RemoteRegisterPath = identityProvider.RemoteRegisterPath;
                });
            }
            else
            {
                services.AddAuthorizationCore();
                services.AddSingleton<AuthenticationStateProvider, EmptyAuthenticationStateProvider>();
                services.AddSingleton<SignOutSessionStateManager>();
            }
        }
    }
}

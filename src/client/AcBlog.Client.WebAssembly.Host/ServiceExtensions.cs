using AcBlog.Client.WebAssembly.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace AcBlog.Client.WebAssembly.Host
{
    public static class ServiceExtensions
    {
        public static void AddServerPrerenderAuthorization(this IServiceCollection services)
        {
            services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            services.AddScoped<SignOutSessionStateManager>();
        }
    }
}

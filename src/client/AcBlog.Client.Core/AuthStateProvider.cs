using AcBlog.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace AcBlog.Client
{
    public class AuthStateProvider : AuthenticationStateProvider, IDisposable
    {
        public AuthStateProvider(IBlogService service, AccessTokenProvider accessTokenProvider)
        {
            Service = service;
            AccessTokenProvider = accessTokenProvider;
            AccessTokenProvider.TokenChanged += AccessTokenProvider_TokenChanged;
        }

        private void AccessTokenProvider_TokenChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        IBlogService Service { get; }
        AccessTokenProvider AccessTokenProvider { get; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsPrincipal claims = null;
            try
            {
                var token = await AccessTokenProvider.GetToken();
                Service.Context.Token = token;
                var user = await Service.UserService.GetCurrent();
                if (user is not null)
                {
                    claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.NameIdentifier, user.Id),
                            new Claim(ClaimTypes.Email, user.Email),
                        }, "acblog-auth"));
                }
            }
            catch
            {
            }
            return new AuthenticationState(claims ?? new ClaimsPrincipal(new ClaimsIdentity()));

        }

        public void Dispose()
        {
            AccessTokenProvider.TokenChanged -= AccessTokenProvider_TokenChanged;
        }
    }
}

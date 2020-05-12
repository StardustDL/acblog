using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AcBlog.Client.WebAssembly.Models
{
    public class EmptyAuthenticationStateProvider : AuthenticationStateProvider
    {
        static AuthenticationState State { get; set; } = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(State);
        }
    }
}

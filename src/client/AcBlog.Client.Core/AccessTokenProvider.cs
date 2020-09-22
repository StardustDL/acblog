using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web.Extensions;
using Microsoft.JSInterop;

namespace AcBlog.Client
{
    public class AccessTokenProvider
    {
        public AccessTokenProvider(IJSRuntime jSRuntime) => JSRuntime = jSRuntime;

        /*public AccessTokenProvider(ProtectedLocalStorage protectedLocalStorage)
{
ProtectedLocalStorage = protectedLocalStorage;
}

ProtectedLocalStorage ProtectedLocalStorage { get; }*/

        IJSRuntime JSRuntime { get; }

        public event Action TokenChanged;

        public async Task<string> GetToken()
        {
            try
            {
                return await JSRuntime.InvokeAsync<string>("localStorage.getItem", "access_token");
            }
            catch
            {
                return "";
            }
        }

        public async Task SetToken(string token)
        {
            await JSRuntime.InvokeVoidAsync("localStorage.setItem", "access_token", token);

            TokenChanged?.Invoke();
        }
    }
}

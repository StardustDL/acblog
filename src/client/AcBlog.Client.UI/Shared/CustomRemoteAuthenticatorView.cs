using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace AcBlog.Client.UI.Shared
{
    public class CustomRemoteAuthenticatorView : RemoteAuthenticatorViewCore<RemoteAuthenticationState>
    {
        [Inject]
        private IJSRuntime JS { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private SignOutSessionStateManager SignOutManager { get; set; }

        public CustomRemoteAuthenticatorView() => AuthenticationState = new RemoteAuthenticationState();

        private static void RegisterNotSupportedFragment(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "Registration is not supported.");
            builder.CloseElement();
        }

        private static void ProfileNotSupportedFragment(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "Editing the profile is not supported.");
            builder.CloseElement();
        }

        private ValueTask RedirectToRegister()
        {
            string pathAndQuery = Navigation.ToAbsoluteUri(ApplicationPaths.LogInPath).PathAndQuery;
            string pathAndQuery2 = Navigation.ToAbsoluteUri(ApplicationPaths.RemoteRegisterPath + "?returnUrl=" + Uri.EscapeDataString(pathAndQuery)).ToString();
            return JS.InvokeVoidAsync("location.replace", pathAndQuery2);
        }

        private ValueTask RedirectToProfile()
        {
            return JS.InvokeVoidAsync("location.replace", Navigation.ToAbsoluteUri(ApplicationPaths.RemoteProfilePath).ToString());
        }

        protected override async Task OnParametersSetAsync()
        {
            switch (Action)
            {
                case "profile":
                    if (ApplicationPaths.RemoteProfilePath is null)
                    {
                        UserProfile ??= ProfileNotSupportedFragment;
                        break;
                    }
                    UserProfile ??= LoggingIn;
                    await RedirectToProfile();
                    break;
                case "register":
                    if (ApplicationPaths.RemoteRegisterPath is null)
                    {
                        Registering ??= RegisterNotSupportedFragment;
                        break;
                    }
                    Registering ??= LoggingIn;
                    await RedirectToRegister();
                    break;
                case "logout":
                    await SignOutManager.SetSignOutState();
                    await base.OnParametersSetAsync();
                    break;
                default:
                    await base.OnParametersSetAsync();
                    break;
            }
        }
    }
}

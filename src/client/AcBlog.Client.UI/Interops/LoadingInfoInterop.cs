using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace AcBlog.Client.UI.Interops
{
    public static class LoadingInfoInterop
    {
        public static ValueTask Show(IJSRuntime runtime)
        {
            return runtime.InvokeVoidAsync("acblogInteropLoadingInfoShow");
        }

        public static ValueTask Hide(IJSRuntime runtime)
        {
            return runtime.InvokeVoidAsync("acblogInteropLoadingInfoHide");
        }
    }
}

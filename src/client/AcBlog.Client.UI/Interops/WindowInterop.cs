using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace AcBlog.Client.UI.Interops
{
    public static class WindowInterop
    {
        public static ValueTask SetTitle(IJSRuntime runtime, string title)
        {
            return runtime.InvokeVoidAsync("acblogInteropSetTitle", title);
        }

        public static ValueTask ScrollTo(IJSRuntime runtime, string title)
        {
            return runtime.InvokeVoidAsync("acblogInteropScrollTo", title);
        }

        public static ValueTask CopyItem(IJSRuntime runtime, ElementReference element)
        {
            return runtime.InvokeVoidAsync("acblogInteropCopyItem", element);
        }
    }
}

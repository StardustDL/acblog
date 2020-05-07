using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcBlog.Client.WebAssembly.Interops
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
    }
}

using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace AcBlog.Client.WASM.Interops
{
    public static class ModalInterop
    {
        public static ValueTask Show(IJSRuntime runtime, string id)
        {
            return runtime.InvokeVoidAsync("acblogInteropModalAction", id, "show");
        }

        public static ValueTask Hide(IJSRuntime runtime, string id)
        {
            return runtime.InvokeVoidAsync("acblogInteropModalAction", id, "hide");
        }
    }
}

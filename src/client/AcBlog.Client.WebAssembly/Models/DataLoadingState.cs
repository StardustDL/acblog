using System;

namespace AcBlog.Client.WebAssembly.Models
{
    public enum DataLoadingState
    {
        Initialized,
        Loading,
        Success,
        Failed
    }

    [Flags]
    public enum MessageModalButtons
    {
        Ok = 1,
        Cancel = 2,
        Yes = 4,
        No = 8,
    }
}
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Client.WebAssembly.Host
{
    public static class Utils
    {
        public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            var delayTask = Task.Delay(timeout, timeoutCancellationTokenSource.Token);
            if (await Task.WhenAny(task, delayTask) == task)
            {
                timeoutCancellationTokenSource.Cancel();
                return await task;
            }
            throw new TimeoutException("The operation has timed out.");
        }

        public static string GetBaseAddress(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("BaseAddress");
        }
    }
}

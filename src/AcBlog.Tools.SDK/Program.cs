using AcBlog.Tools.SDK.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AcBlog.Tools.SDK
{
    class Program
    {
        static Workspace? Workspace { get; set; }

        internal static Workspace Current()
        {
            if (Workspace == null)
                throw new Exception("Workspace hasn't been loaded.");
            return Workspace;
        }

        static Task<int> Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            return Task.FromResult(0);
        }
    }
}

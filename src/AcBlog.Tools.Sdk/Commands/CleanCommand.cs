using AcBlog.Tools.Sdk.Helpers;
using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands
{
    public class CleanCommand : BaseCommand<CleanCommand.CArgument>
    {
        public override string Name => "clean";

        public override string Description => "Clean temp files.";

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();
            await workspace.Clean();
            return 0;
        }

        public class CArgument
        {

        }
    }
}

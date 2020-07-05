using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.Hosting;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands
{
    public class LogoutCommand : BaseCommand<LogoutCommand.CArgument>
    {
        public override string Name => "logout";

        public override string Description => "Logout AcBlog server.";

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = Program.Workspace;
            workspace.Configuration.Token = "";
            await workspace.Save();
            return 0;
        }

        public class CArgument
        {
        }
    }
}

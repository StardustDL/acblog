using AcBlog.Tools.SDK.Models;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.SDK.Commands
{
    public class LogoutCommand : BaseCommand<LogoutCommand.CArgument>
    {
        public override string Name => "logout";

        public override string Description => "Logout AcBlog server.";

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
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

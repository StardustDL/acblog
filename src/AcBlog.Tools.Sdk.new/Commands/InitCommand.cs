using AcBlog.Tools.Sdk.Helpers;
using AcBlog.Tools.Sdk.Models;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands
{
    public class InitCommand : BaseCommand<InitCommand.CArgument>
    {
        public override string Name => "init";

        public override string Description => "Initialize AcBlog.";

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
        {
            Workspace workspace = Program.Workspace;
            workspace.Configuration.Remote = null;
            workspace.Configuration.Token = "";
            await workspace.Save();
            {
                DirectoryInfo di = new DirectoryInfo(workspace.GetPostRoot());
                if (!di.Exists) di.Create();
            }
            {
                DirectoryInfo di = new DirectoryInfo(workspace.GetCategoryRoot());
                if (!di.Exists) di.Create();
            }
            {
                DirectoryInfo di = new DirectoryInfo(workspace.GetKeywordRoot());
                if (!di.Exists) di.Create();
            }
            return 0;
        }

        public class CArgument
        {

        }
    }
}

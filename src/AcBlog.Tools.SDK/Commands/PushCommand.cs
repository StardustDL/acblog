using AcBlog.Tools.SDK.Models;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.SDK.Commands
{
    public class PushCommand : BaseCommand<PushCommand.CArgument>
    {
        public override string Name => "push";

        public override string Description => "Push data to server.";

        public override Command Configure()
        {
            var result = base.Configure();
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
        {
            Workspace workspace = Program.Current();
            using var client = new HttpClient();
            await workspace.Connect(client);
            await workspace.Login();
            return 0;
        }

        public class CArgument
        {
        }
    }
}

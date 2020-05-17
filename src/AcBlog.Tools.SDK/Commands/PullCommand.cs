using AcBlog.Tools.SDK.Models;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.SDK.Commands
{
    public class PullCommand : BaseCommand<PullCommand.CArgument>
    {
        public override string Name => "pull";

        public override string Description => "Pull data from server.";

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

            return 0;
        }

        public class CArgument
        {
        }
    }
}

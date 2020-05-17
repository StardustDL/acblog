using AcBlog.Tools.SDK.Models;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.SDK.Commands
{
    public class FetchCommand : BaseCommand<FetchCommand.CArgument>
    {
        public override string Name => "fetch";

        public override string Description => "Fetch data updating from server.";

        public override Command Configure()
        {
            var result = base.Configure();
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
        {
            Workspace workspace = Program.Workspace;
            using var client = new HttpClient();
            await workspace.Connect(client);

            return 0;
        }

        public class CArgument
        {
        }
    }
}

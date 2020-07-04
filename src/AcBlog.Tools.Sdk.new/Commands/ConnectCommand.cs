using AcBlog.Tools.Sdk.Models;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands
{
    public class ConnectCommand : BaseCommand<ConnectCommand.CArgument>
    {
        public override string Name => "connect";

        public override string Description => "Connect to AcBlog server.";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddArgument(new Argument<Uri>(nameof(CArgument.Uri).ToLowerInvariant(), "Remote server URI"));
            result.AddOption(new Option<bool>($"--{nameof(CArgument.Static).ToLowerInvariant()}", "If the remote server is static-file server"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
        {
            Workspace workspace = Program.Workspace;
            workspace.Configuration.Remote = new ServerConfiguration(argument.Uri!, argument.Static);
            using var client = new HttpClient();
            await workspace.Connect(client);
            await workspace.Save();
            return 0;
        }

        public class CArgument
        {
            public Uri? Uri { get; set; }

            public bool Static { get; set; }
        }
    }
}

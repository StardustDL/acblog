using AcBlog.Tools.SDK.Models;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.SDK.Commands
{
    public class LoginCommand : BaseCommand<LoginCommand.CArgument>
    {
        public override string Name => "login";

        public override string Description => "Login AcBlog server.";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddArgument(new Argument<string>(nameof(CArgument.Token).ToLowerInvariant(), "User token"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
        {
            Workspace workspace = Program.Workspace;
            workspace.Configuration.Token = argument.Token!;
            using var client = new HttpClient();
            await workspace.Connect(client);
            await workspace.Login();
            await workspace.Save();
            return 0;
        }

        public class CArgument
        {
            public string? Token { get; set; }
        }
    }
}

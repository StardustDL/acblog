using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.CommandLine;
using System.CommandLine.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands
{
    public class RemoteCommand : BaseCommand<RemoteCommand.CArgument>
    {
        public override string Name => "remote";

        public override string Description => "";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddCommand(new Remotes.AddCommand().Build());
            result.AddCommand(new Remotes.RemoveCommand().Build());
            result.AddCommand(new Remotes.ConfigCommand().Build());
            return result;
        }

        public override Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();
            IConsole console = host.Services.GetRequiredService<IConsole>();
            console.Out.WriteLine($"Current: {workspace.Option.CurrentRemote}");
            foreach (var item in workspace.Option.Remotes.Values)
            {
                console.Out.WriteLine($"{item.Name} ({Enum.GetName(typeof(RemoteType), item.Type)}): {item.Uri}");
            }
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}

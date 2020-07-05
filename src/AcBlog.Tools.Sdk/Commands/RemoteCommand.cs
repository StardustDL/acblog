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
            result.AddArgument(new Argument<string?>(nameof(CArgument.Current).ToLowerInvariant(), () => null, "Current remote name"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();
            if (argument.Current is null)
            {
                Console.WriteLine($"Current: {workspace.Option.CurrentRemote}");
                foreach (var item in workspace.Option.Remotes.Values)
                {
                    Console.WriteLine($"{item.Name} ({Enum.GetName(typeof(RemoteType), item.Type)}): {item.Uri}");
                }
            }
            else
            {
                workspace.Option.CurrentRemote = argument.Current;
                await workspace.Save();
            }
            return 0;
        }

        public class CArgument
        {
            public string? Current { get; set; }
        }
    }
}

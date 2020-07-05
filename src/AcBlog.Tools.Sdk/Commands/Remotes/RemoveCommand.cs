using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands.Remotes
{
    public class RemoveCommand : BaseCommand<RemoveCommand.CArgument>
    {
        public override string Name => "remove";

        public override string Description => "";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddArgument(new Argument<string>(nameof(CArgument.Name).ToLowerInvariant(), "Remote server name"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();
            workspace.Option.Remotes.Remove(argument.Name);
            if (workspace.Option.CurrentRemote == argument.Name)
                workspace.Option.CurrentRemote = string.Empty;
            await workspace.Save();
            return 0;
        }

        public class CArgument
        {
            public string Name { get; set; } = string.Empty;
        }
    }
}

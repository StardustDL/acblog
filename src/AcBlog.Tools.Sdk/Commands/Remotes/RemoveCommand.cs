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
    public class AddCommand : BaseCommand<AddCommand.CArgument>
    {
        public override string Name => "add";

        public override string Description => "";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddArgument(new Argument<string>(nameof(CArgument.Name).ToLowerInvariant(), "Remote server name"));
            result.AddArgument(new Argument<string>(nameof(CArgument.Uri).ToLowerInvariant(), "Remote server URI"));
            result.AddOption(new Option<RemoteType>($"--{nameof(CArgument.Type).ToLowerInvariant()}", "Type"));
            result.AddOption(new Option<string>($"--{nameof(CArgument.Token).ToLowerInvariant()}", "Token"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();
            workspace.Option.Remotes.Add(argument.Name, new RemoteOption
            {
                Name = argument.Name,
                Uri = argument.Uri,
                Type = argument.Type,
                Token = argument.Token
            });
            if (string.IsNullOrEmpty(workspace.Option.CurrentRemote))
                workspace.Option.CurrentRemote = argument.Name;
            await workspace.Save();
            return 0;
        }

        public class CArgument
        {
            public string Uri { get; set; } = string.Empty;

            public string Name { get; set; } = string.Empty;

            public string Token { get; set; } = string.Empty;

            public RemoteType Type { get; set; }
        }
    }
}

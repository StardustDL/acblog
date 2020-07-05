using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands.Remotes
{
    public class ConfigCommand : BaseCommand<ConfigCommand.CArgument>
    {
        public override string Name => "config";

        public override string Description => "";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddArgument(new Argument<string>(nameof(CArgument.Name).ToLowerInvariant(), "Remote server name"));
            result.AddArgument(new Argument<string?>(nameof(CArgument.Uri).ToLowerInvariant(), () => null, "Remote server URI"));
            result.AddOption(new Option<RemoteType?>($"--{nameof(CArgument.Type).ToLowerInvariant()}", () => null, "Type"));
            result.AddOption(new Option<string?>($"--{nameof(CArgument.Token).ToLowerInvariant()}", () => null, "Token"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();
            if (workspace.Option.Remotes.TryGetValue(argument.Name, out var remote))
            {
                if (argument.Token != null)
                    remote.Token = argument.Token;
                if (argument.Type != null)
                    remote.Type = argument.Type.Value;
                if (argument.Uri != null)
                {
                    if (remote.Type == RemoteType.LocalFS)
                    {
                        remote.Uri = new DirectoryInfo(argument.Uri).FullName;
                    }
                    else
                    {
                        remote.Uri = argument.Uri;
                    }
                }
            }
            await workspace.Save();
            return 0;
        }

        public class CArgument
        {
            public string? Uri { get; set; } = null;

            public string Name { get; set; } = string.Empty;

            public string? Token { get; set; } = null;

            public RemoteType? Type { get; set; } = null;
        }
    }
}

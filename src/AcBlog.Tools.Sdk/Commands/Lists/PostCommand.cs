using AcBlog.Data.Models;
using AcBlog.Sdk;
using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands.Lists
{
    public class PostCommand : BaseCommand<PostCommand.CArgument>
    {
        public override string Name => "post";

        public override string Description => "List posts.";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddOption(new Option<bool>($"--{nameof(CArgument.Remote).ToLowerInvariant()}", "Remote"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();

            IBlogService service;
            if (argument.Remote)
            {
                await workspace.Connect();
                service = workspace.Remote;
            }
            else
            {
                service = workspace.Local;
            }

            foreach (var id in await service.PostService.All(cancellationToken))
            {
                var item = await service.PostService.Get(id, cancellationToken);
                if (item != null)
                {
                    Console.WriteLine(item.Title);
                }
            }
            return 0;
        }

        public class CArgument
        {
            public bool Remote { get; set; }
        }
    }
}

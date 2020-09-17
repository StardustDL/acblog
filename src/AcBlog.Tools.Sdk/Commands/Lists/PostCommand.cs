using AcBlog.Sdk;
using AcBlog.Services;
using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.CommandLine;
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

            await foreach (var id in service.PostService.All(cancellationToken))
            {
                var item = await service.PostService.Get(id, cancellationToken);
                if (item is not null)
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

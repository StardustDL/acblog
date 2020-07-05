using AcBlog.Data.Models;
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

namespace AcBlog.Tools.Sdk.Commands
{
    public class ListPostCommand : BaseCommand<ListPostCommand.CArgument>
    {
        public override string Name => "post";

        public override string Description => "List posts.";

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();
            await workspace.Connect();
            Console.WriteLine(workspace.BlogService);

            /*Workspace workspace = Program.Workspace;
            using var client = new HttpClient();
            await workspace.Connect(client);
            var service = workspace.Remote!.PostService;
            var list = (await service.All(cancellationToken)).ToList();
            console.Out.WriteLine($"Founded {list.Count} posts.");
            var items = await Task.WhenAll(list.Select(id => service.Get(id, cancellationToken)));
            foreach (var item in items)
            {
                cancellationToken.ThrowIfCancellationRequested();
                console.Out.WriteLine(item!.Title);
            }
            return 0;*/
            return 0;
        }

        public class CArgument
        {
        }
    }
}

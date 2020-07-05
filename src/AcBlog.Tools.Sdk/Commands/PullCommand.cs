using AcBlog.Data.Models;
using AcBlog.Tools.Sdk.Helpers;
using AcBlog.Tools.Sdk.Models;
using AcBlog.Tools.Sdk.Models.Text;
using Microsoft.Extensions.Hosting;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands
{
    public class PullCommand : BaseCommand<PullCommand.CArgument>
    {
        public override string Name => "pull";

        public override string Description => "Pull data from server.";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddOption(new Option<bool>($"--{nameof(CArgument.Force).ToLowerInvariant()}", "Force pull"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = Program.Workspace;
            using var client = new HttpClient();
            await workspace.Connect(client);
            if (workspace.Remote is null)
                return -1;
            if (argument.Force)
                workspace.History.Clear();
            {
                PostTextual text = new PostTextual();
                var service = workspace.Remote.PostService;
                console.Out.WriteLine("Pull posts...");
                foreach (var id in await service.All(cancellationToken))
                {
                    console.Out.Write($"{id}: ");
                    cancellationToken.ThrowIfCancellationRequested();
                    var p = (await service.Get(id, cancellationToken))!;
                    if (workspace.History.TryGetValue(id, out var dbitem))
                    {
                        var old = JsonSerializer.Deserialize<Post>(dbitem.RemoteHash);
                        if (old.EqualsTo(p))
                        {
                            console.Out.WriteLine("Up to date.");
                            continue;
                        }
                    }
                    string path = workspace.GenerateItemPath(p);
                    await File.WriteAllTextAsync(path, text.Format(p), cancellationToken);
                    workspace.History[id] = new DbItem
                    {
                        RemoteHash = JsonSerializer.Serialize(p),
                        LastUpdateTime = DateTimeOffset.Now
                    };
                    console.Out.WriteLine("Updated.");
                }
            }
            {
                CategoryTextual text = new CategoryTextual();
                var service = workspace.Remote.CategoryService;
                console.Out.WriteLine("Pull categories...");
                foreach (var id in await service.All(cancellationToken))
                {
                    console.Out.Write($"{id}: ");
                    cancellationToken.ThrowIfCancellationRequested();
                    var p = (await service.Get(id, cancellationToken))!;
                    if (workspace.History.TryGetValue(id, out var dbitem))
                    {
                        var old = JsonSerializer.Deserialize<Category>(dbitem.RemoteHash);
                        if (old.EqualsTo(p))
                        {
                            console.Out.WriteLine("Up to date.");
                            continue;
                        }
                    }
                    string path = workspace.GenerateItemPath(p);
                    await File.WriteAllTextAsync(path, text.Format(p), cancellationToken);
                    workspace.History[id] = new DbItem
                    {
                        RemoteHash = JsonSerializer.Serialize(p),
                        LastUpdateTime = DateTimeOffset.Now
                    };
                    console.Out.WriteLine("Updated.");
                }
            }
            {
                KeywordTextual text = new KeywordTextual();
                var service = workspace.Remote.KeywordService;
                console.Out.WriteLine("Pull keywords...");
                foreach (var id in await service.All(cancellationToken))
                {
                    console.Out.Write($"{id}: ");
                    cancellationToken.ThrowIfCancellationRequested();
                    var p = (await service.Get(id, cancellationToken))!;
                    if (workspace.History.TryGetValue(id, out var dbitem))
                    {
                        var old = JsonSerializer.Deserialize<Keyword>(dbitem.RemoteHash);
                        if (old.EqualsTo(p))
                        {
                            console.Out.WriteLine("Up to date.");
                            continue;
                        }
                    }
                    string path = workspace.GenerateItemPath(p);
                    await File.WriteAllTextAsync(path, text.Format(p), cancellationToken);
                    workspace.History[id] = new DbItem
                    {
                        RemoteHash = JsonSerializer.Serialize(p),
                        LastUpdateTime = DateTimeOffset.Now
                    };
                    console.Out.WriteLine("Updated.");
                }
            }
            await workspace.Save();

            return 0;
        }

        public class CArgument
        {
            public bool Force { get; set; } = false;
        }
    }
}

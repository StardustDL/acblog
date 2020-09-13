using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands
{
    /*
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

    public class PushCommand : BaseCommand<PushCommand.CArgument>
    {
        public override string Name => "push";

        public override string Description => "Push data to server.";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddOption(new Option<bool>($"--{nameof(CArgument.Force).ToLowerInvariant()}", "Force push"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = Program.Workspace;
            using var client = new HttpClient();
            await workspace.Connect(client);
            await workspace.Login();
            if (workspace.Remote is null)
                return -1;
            {
                var text = new PostTextual();
                var service = workspace.Remote.PostService;
                console.Out.WriteLine("Push posts...");
                foreach (var file in workspace.GetPostFiles())
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var p = text.Parse(await File.ReadAllTextAsync(file, cancellationToken));
                    if (workspace.History.TryGetValue(p.Id, out var dbitem))
                    {
                        console.Out.Write($"{p.Id}: ");
                        if (!argument.Force && dbitem.LastUpdateTime >= File.GetLastWriteTimeUtc(file))
                        {
                            console.Out.WriteLine("Up to date.");
                            continue;
                        }
                        if(!await service.Update(p, cancellationToken))
                        {
                            console.Out.WriteLine("Failed to update.");
                            continue;
                        }
                    }
                    else
                    {
                        console.Out.Write($"@New({p.Title}): ");
                        var id = await service.Create(p, cancellationToken);
                        if(id is null)
                        {
                            console.Out.WriteLine("Failed to create.");
                            continue;
                        }
                        else
                        {
                            p.Id = id;
                            string path = workspace.GenerateItemPath(p);
                            await File.WriteAllTextAsync(path, text.Format(p), cancellationToken);
                        }
                    }

                    workspace.History[p.Id] = new DbItem
                    {
                        RemoteHash = JsonSerializer.Serialize(p),
                        LastUpdateTime = DateTimeOffset.Now
                    };
                    console.Out.WriteLine("Updated.");
                }
            }

            {
                var text = new CategoryTextual();
                var service = workspace.Remote.CategoryService;
                console.Out.WriteLine("Push categories...");
                foreach (var file in workspace.GetCategoryFiles())
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var p = text.Parse(await File.ReadAllTextAsync(file, cancellationToken));
                    if (workspace.History.TryGetValue(p.Id, out var dbitem))
                    {
                        console.Out.Write($"{p.Id}: ");
                        if (!argument.Force && dbitem.LastUpdateTime >= File.GetLastWriteTimeUtc(file))
                        {
                            console.Out.WriteLine("Up to date.");
                            continue;
                        }
                        if (!await service.Update(p, cancellationToken))
                        {
                            console.Out.WriteLine("Failed to update.");
                            continue;
                        }
                    }
                    else
                    {
                        console.Out.Write($"@New({p.Name}): ");
                        var id = await service.Create(p, cancellationToken);
                        if (id is null)
                        {
                            console.Out.WriteLine("Failed to create.");
                            continue;
                        }
                        else
                        {
                            p.Id = id;
                            string path = workspace.GenerateItemPath(p);
                            await File.WriteAllTextAsync(path, text.Format(p), cancellationToken);
                        }
                    }

                    workspace.History[p.Id] = new DbItem
                    {
                        RemoteHash = JsonSerializer.Serialize(p),
                        LastUpdateTime = DateTimeOffset.Now
                    };
                    console.Out.WriteLine("Updated.");
                }
            }

            {
                var text = new KeywordTextual();
                var service = workspace.Remote.KeywordService;
                console.Out.WriteLine("Push keywords...");
                foreach (var file in workspace.GetKeywordFiles())
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var p = text.Parse(await File.ReadAllTextAsync(file, cancellationToken));
                    if (workspace.History.TryGetValue(p.Id, out var dbitem))
                    {
                        console.Out.Write($"{p.Id}: ");
                        if (!argument.Force && dbitem.LastUpdateTime >= File.GetLastWriteTimeUtc(file))
                        {
                            console.Out.WriteLine("Up to date.");
                            continue;
                        }
                        if (!await service.Update(p, cancellationToken))
                        {
                            console.Out.WriteLine("Failed to update.");
                            continue;
                        }
                    }
                    else
                    {
                        console.Out.Write($"@New({p.Name}): ");
                        var id = await service.Create(p, cancellationToken);
                        if (id is null)
                        {
                            console.Out.WriteLine("Failed to create.");
                            continue;
                        }
                        else
                        {
                            p.Id = id;
                            File.Delete(file);
                            string path = workspace.GenerateItemPath(p);
                            await File.WriteAllTextAsync(path, text.Format(p), cancellationToken);
                        }
                    }

                    workspace.History[p.Id] = new DbItem
                    {
                        RemoteHash = JsonSerializer.Serialize(p),
                        LastUpdateTime = DateTimeOffset.Now
                    };
                    console.Out.WriteLine("Updated.");
                }
            }

            return 0;
        }

        public class CArgument
        {
            public bool Force { get; set; } = false;
        }
    }

    */

    public class PushCommand : BaseCommand<PushCommand.CArgument>
    {
        public override string Name => "push";

        public override string Description => "Push data to server.";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddOption(new Option<bool>($"--{nameof(CArgument.Full).ToLowerInvariant()}", "Delete diff data (only for API remote)"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();
            await workspace.Push(full: argument.Full);
            return 0;
        }

        public class CArgument
        {
            public bool Full { get; set; }
        }
    }
}

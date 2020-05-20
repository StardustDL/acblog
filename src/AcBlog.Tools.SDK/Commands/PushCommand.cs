using AcBlog.Tools.SDK.Helpers;
using AcBlog.Tools.SDK.Models;
using AcBlog.Tools.SDK.Models.Text;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.SDK.Commands
{
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

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, CancellationToken cancellationToken)
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
}

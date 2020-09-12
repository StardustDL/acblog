using AcBlog.Data.Models;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using AcBlog.Sdk;
using AcBlog.Sdk.Api;
using AcBlog.Sdk.FileSystem;
using AcBlog.Tools.Sdk.Helpers;
using AcBlog.Tools.Sdk.Repositories;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StardustDL.Extensions.FileProviders;
using StardustDL.Extensions.FileProviders.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AcBlog.Sdk.Sitemap;
using AcBlog.Sdk.Syndication;
using System.Xml;
using AcBlog.Data.Repositories.FileSystem.Builders;
using AcBlog.Data.Extensions;

namespace AcBlog.Tools.Sdk.Models
{
    public class Workspace
    {
        public const string OptionPath = "acblog.json";

        public const string DBPath = "db.json";

        public const string BlogOptionPath = "blog.json";

        const string AssetsPath = "assets";

        public Workspace(IOptions<WorkspaceOption> option, IOptions<DB> db, LocalBlogService localBlogService, IHttpClientFactory httpClientFactory, ILogger<Workspace> logger)
        {
            HttpClientFactory = httpClientFactory;
            Logger = logger;
            Remote = new FileSystemBlogService(
                new PhysicalFileProvider(Environment.CurrentDirectory).AsFileProvider());
            Local = localBlogService;
            Option = option.Value;
            DB = db.Value;
        }

        public WorkspaceOption Option { get; private set; }

        public DB DB { get; private set; }

        private IHttpClientFactory HttpClientFactory { get; }

        private ILogger<Workspace> Logger { get; }

        async Task SaveDb()
        {
            await using var st = System.IO.File.Open(DBPath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(st, new { db = DB }, options: new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        async Task SaveOption()
        {
            await using var st = System.IO.File.Open(OptionPath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(st, new { acblog = Option }, options: new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        public IBlogService Remote { get; private set; }

        public LocalBlogService Local { get; private set; }


        // TODO: fix rewrite behavior
        public async Task Initialize()
        {
            Option = new WorkspaceOption();
            DB = new DB();
            await Save();
            FSBuilder builder = new FSBuilder(Environment.CurrentDirectory);
            builder.EnsureDirectoryExists("posts");
            builder.EnsureDirectoryExists("pages");
            builder.EnsureDirectoryExists("layouts");
            builder.EnsureDirectoryExists(AssetsPath);

            {
                await using var st = builder.GetFileRewriteStream(BlogOptionPath);
                await JsonSerializer.SerializeAsync(st, new BlogOptions(), options: new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }

            await Clean();
        }

        public async Task Save()
        {
            Logger.LogInformation("Save data.");
            await SaveOption();
            await SaveDb();
        }

        const string GitTempFolder = "temp/git";

        public async Task Connect(string name = "")
        {
            if (string.IsNullOrEmpty(name))
                name = Option.CurrentRemote;

            Logger.LogInformation($"Connect to remote {name}.");

            if (Option.Remotes.TryGetValue(name, out var remote))
            {
                Logger.LogInformation($"Detect remote {remote.Name} ({Enum.GetName(typeof(RemoteType), remote.Type)}).");
                switch (remote.Type)
                {
                    case RemoteType.LocalFS:
                        Remote = new FileSystemBlogService(
                            new PhysicalFileProvider(remote.Uri).AsFileProvider());
                        break;
                    case RemoteType.RemoteFS:
                        {
                            var client = HttpClientFactory.CreateClient();
                            client.BaseAddress = new Uri(remote.Uri);
                            Remote = new FileSystemBlogService(
                                new HttpFileProvider(client));
                        }
                        break;
                    case RemoteType.Api:
                        {
                            var client = HttpClientFactory.CreateClient();
                            client.BaseAddress = new Uri(remote.Uri);
                            Remote = new ApiBlogService(client);
                        }
                        break;
                    case RemoteType.Git:
                        {
                            FSBuilder builder = new FSBuilder(Environment.CurrentDirectory);

                            Logger.LogInformation("Pull git repository.");

                            try
                            {
                                using var repo = new Repository(GitTempFolder);
                                // Credential information to fetch
                                LibGit2Sharp.PullOptions options = new LibGit2Sharp.PullOptions();

                                // User information to create a merge commit
                                var signature = new LibGit2Sharp.Signature(
                                    new Identity("AcBlog.Tools.Sdk", "tools.sdk@acblog"), DateTimeOffset.Now);

                                // Pull
                                LibGit2Sharp.Commands.Pull(repo, signature, options);
                            }
                            catch
                            {
                                builder.EnsureDirectoryEmpty(GitTempFolder);
                                Repository.Clone(remote.Uri, GitTempFolder);
                            }

                            Remote = new FileSystemBlogService(
                                new PhysicalFileProvider(Path.Join(Environment.CurrentDirectory, GitTempFolder)).AsFileProvider());
                        }
                        break;
                }
                Remote.PostService.Context.Token = remote.Token;

                Option.CurrentRemote = name;
                await SaveOption();
            }
            else
            {
                throw new Exception("No remote");
            }
        }

        // full: delete diff post for api remote
        public async Task Push(string name = "", bool full = false)
        {
            if (string.IsNullOrEmpty(name))
                name = Option.CurrentRemote;

            Logger.LogInformation($"Push to remote {name}.");

            if (Option.Remotes.TryGetValue(name, out var remote))
            {
                Logger.LogInformation($"Detect remote {remote.Name} ({Enum.GetName(typeof(RemoteType), remote.Type)}).");
                switch (remote.Type)
                {
                    case RemoteType.LocalFS:
                        {
                            await toLocalFS(remote);
                        }
                        break;
                    case RemoteType.RemoteFS:
                        {
                            throw new NotSupportedException("Not support pushing to remote file system, please push to local file system and sync to remote.");
                        }
                    case RemoteType.Api:
                        {
                            await Connect(name);
                            Logger.LogInformation($"Fetch remote posts.");

                            // TODO: Pages & layouts

                            HashSet<string> remoteIds = (await Remote.PostService.All()).ToHashSet();
                            foreach (var item in await Local.PostService.GetAllItems())
                            {
                                if (item is null)
                                    continue;
                                Logger.LogInformation($"Loaded {item.Id}: {item.Title}");
                                if (remoteIds.Contains(item.Id))
                                {
                                    var result = await Remote.PostService.Update(item);
                                    if (result)
                                    {
                                        Logger.LogInformation($"Updated {item.Id}");
                                    }
                                    else
                                    {
                                        Logger.LogError($"Failed to update {item.Id}");
                                    }
                                }
                                else
                                {
                                    var result = await Remote.PostService.Create(item);
                                    if (result is null)
                                    {
                                        Logger.LogError($"Failed to create {item.Id}");
                                    }
                                    else
                                    {
                                        Logger.LogInformation($"Created {item.Id}");
                                    }
                                }
                                remoteIds.Remove(item.Id);
                            }
                            if (full)
                            {
                                foreach (var v in remoteIds)
                                {
                                    var result = await Remote.PostService.Delete(v);
                                    if (result)
                                    {
                                        Logger.LogInformation($"Deleted {v}.");
                                    }
                                    else
                                    {
                                        Logger.LogError($"Failed to deleted {v}.");
                                    }
                                }
                            }
                        }
                        break;
                    case RemoteType.Git:
                        {
                            await Connect(name);

                            string tempDist = Path.Join(Environment.CurrentDirectory, "temp/dist");

                            Logger.LogInformation("Generate data.");

                            await toLocalFS(new RemoteOption
                            {
                                Uri = tempDist,
                                Type = RemoteType.LocalFS,
                                Name = remote.Name
                            });

                            FSExtensions.CopyDirectory(tempDist, GitTempFolder);

                            Logger.LogInformation("Load git config.");

                            string userName = Option.Properties[$"remote.{remote.Name}.git.username"],
                                password = Option.Properties[$"remote.{remote.Name}.git.password"];

                            {
                                if (string.IsNullOrEmpty(userName))
                                    userName = ConsoleExtensions.Input("Input username: ");
                                if (string.IsNullOrEmpty(password))
                                    password = ConsoleExtensions.InputPassword("Input password: ");
                            }

                            using (var repo = new Repository(GitTempFolder))
                            {
                                Logger.LogInformation("Commit to git.");

                                LibGit2Sharp.Commands.Stage(repo, "*");

                                var signature = new LibGit2Sharp.Signature(
                                        new Identity("AcBlog.Tools.Sdk", "tools.sdk@acblog"), DateTimeOffset.Now);
                                repo.Commit(DateTimeOffset.Now.ToString(), signature, signature, new CommitOptions
                                {
                                    AllowEmptyCommit = true
                                });

                                Logger.LogInformation($"Push to {repo.Head.RemoteName}.");

                                PushOptions options = new LibGit2Sharp.PushOptions();
                                options.CredentialsProvider = new CredentialsHandler(
                                    (url, usernameFromUrl, types) =>
                                        new UsernamePasswordCredentials()
                                        {
                                            Username = string.IsNullOrEmpty(userName) ? usernameFromUrl : userName,
                                            Password = password
                                        });
                                repo.Network.Push(repo.Head, options);
                            }
                        }
                        break;
                }
            }
            else
            {
                throw new Exception("No remote");
            }

            async Task toLocalFS(RemoteOption remote)
            {
                FSBuilder fsBuilder = new FSBuilder(remote.Uri);
                fsBuilder.EnsureDirectoryEmpty();

                List<Post> posts = new List<Post>();
                foreach (var item in await Local.PostService.GetAllItems())
                {
                    if (item is null)
                        continue;
                    Logger.LogInformation($"Loaded Post {item.Id}: {item.Title}");
                    posts.Add(item);
                }

                List<Layout> layouts = new List<Layout>();
                foreach (var item in await Local.LayoutService.GetAllItems())
                {
                    if (item is null)
                        continue;
                    Logger.LogInformation($"Loaded Layout {item.Id}");
                    layouts.Add(item);
                }

                List<Page> pages = new List<Page>();
                foreach (var item in await Local.PageService.GetAllItems())
                {
                    if (item is null)
                        continue;
                    Logger.LogInformation($"Loaded Page {item.Id}: {item.Title}");
                    pages.Add(item);
                }

                var baseAddress = Option.Properties[$"remote.{remote.Name}.generator.baseAddress"];

                List<Data.Models.File> files = new List<Data.Models.File>();
                {
                    string path = Path.Join(Environment.CurrentDirectory, AssetsPath);
                    if (Directory.Exists(path))
                    {
                        foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                        {
                            Data.Models.File f = new Data.Models.File();
                            f.Id = Path.GetRelativePath(Environment.CurrentDirectory, file).Replace('\\', '/');
                            if (!string.IsNullOrEmpty(baseAddress))
                            {
                                f.Uri = $"{baseAddress.TrimEnd('/')}/{f.Id}";
                            }
                            else
                            {
                                f.Uri = $"/{f.Id}";
                            }
                            files.Add(f);
                        }
                    }
                }

                Logger.LogInformation("Build data.");
                {
                    BlogOptions options = await Local.GetOptions();
                    BlogBuilder builder = new BlogBuilder(options, Path.Join(remote.Uri));
                    await builder.Build();
                    await builder.BuildPosts(posts);
                    await builder.BuildLayouts(layouts);
                    await builder.BuildPages(pages);
                    await builder.BuildFiles(files);
                }

                {
                    if (!string.IsNullOrEmpty(baseAddress))
                    {
                        Logger.LogInformation("Build sitemap.");
                        var sub = fsBuilder.CreateSubDirectoryBuilder("Site");
                        {
                            var siteMapBuilder = await Local.BuildSitemap(baseAddress);
                            await using var st = sub.GetFileRewriteStream("sitemap.xml");
                            await using var writer = XmlWriter.Create(st);
                            siteMapBuilder.Build().WriteTo(writer);
                        }
                        Logger.LogInformation("Build feed.");
                        {
                            var feed = await Local.BuildSyndication(baseAddress);
                            await using (var st = sub.GetFileRewriteStream("atom.xml"))
                            {
                                await using var writer = XmlWriter.Create(st);
                                feed.GetAtom10Formatter().WriteTo(writer);
                            }
                            await using (var st = sub.GetFileRewriteStream("rss.xml"))
                            {
                                await using var writer = XmlWriter.Create(st);
                                feed.GetRss20Formatter().WriteTo(writer);
                            }
                        }
                    }
                }
                {
                    string assetsPath = Path.Join(Environment.CurrentDirectory, AssetsPath);
                    if (Directory.Exists(assetsPath))
                    {
                        Logger.LogInformation("Copy assets.");
                        FSExtensions.CopyDirectory(assetsPath, Path.Join(remote.Uri, AssetsPath));
                    }
                }
            }
        }

        public Task Clean()
        {
            FSBuilder builder = new FSBuilder(Environment.CurrentDirectory);
            builder.EnsureDirectoryExists("temp", false);
            return Task.CompletedTask;
        }
    }
}

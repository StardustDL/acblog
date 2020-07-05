using AcBlog.Data.Models;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using AcBlog.Sdk;
using AcBlog.Sdk.Api;
using AcBlog.Sdk.FileSystem;
using AcBlog.Tools.Sdk.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using StardustDL.Extensions.FileProviders.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Models
{
    public class Workspace
    {
        public const string OptionPath = "acblog.json";

        public const string DBPath = "db.json";

        public Workspace(IOptions<WorkspaceOption> option, IOptions<DB> db, LocalBlogService localBlogService, IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
            Remote = new FileSystemBlogService(
                new PhysicalFileProvider(Environment.CurrentDirectory));
            Local = localBlogService;
            Option = option.Value;
            DB = db.Value;
        }

        public WorkspaceOption Option { get; private set; }

        public DB DB { get; private set; }

        private IHttpClientFactory HttpClientFactory { get; }

        async Task SaveDb()
        {
            using var st = File.Open(DBPath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(st, new { db = DB }, options: new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        async Task SaveOption()
        {
            using var st = File.Open(OptionPath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(st, new { acblog = Option }, options: new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        public IBlogService Remote { get; private set; }

        public LocalBlogService Local { get; private set; }

        public async Task Initialize()
        {
            Option = new WorkspaceOption();
            DB = new DB();
            await Save();
            FSBuilder builder = new FSBuilder(Environment.CurrentDirectory);
            builder.EnsureDirectoryExists("posts");
        }

        public async Task Save()
        {
            await SaveOption();
            await SaveDb();
        }

        public async Task Connect(string name = "")
        {
            if (string.IsNullOrEmpty(name))
                name = Option.CurrentRemote;

            if (Option.Remotes.TryGetValue(name, out var remote))
            {
                switch (remote.Type)
                {
                    case RemoteType.LocalFS:
                        Remote = new FileSystemBlogService(
                            new PhysicalFileProvider(remote.Uri));
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

            if (Option.Remotes.TryGetValue(name, out var remote))
            {
                switch (remote.Type)
                {
                    case RemoteType.LocalFS:
                    {
                        FSBuilder fsBuilder = new FSBuilder(remote.Uri);
                        fsBuilder.EnsureDirectoryEmpty();

                        List<Post> posts = new List<Post>();
                        foreach (var item in await Local.PostService.GetAllPosts())
                        {
                            if (item is null)
                                continue;
                            posts.Add(item);
                        }
                        PostRepositoryBuilder builder = new PostRepositoryBuilder(posts, Path.Join(remote.Uri, "posts"));
                        await builder.Build();
                    }
                    break;
                    case RemoteType.RemoteFS:
                    {
                        throw new NotSupportedException("Not support pushing to remote file system, please push to local file system and sync to remote.");
                    }
                    case RemoteType.Api:
                    {
                        await Connect(name);
                        HashSet<string> remoteIds = (await Remote.PostService.All()).ToHashSet();
                        foreach (var item in await Local.PostService.GetAllPosts())
                        {
                            if (item is null)
                                continue;
                            if (remoteIds.Contains(item.Id))
                            {
                                var result = await Remote.PostService.Update(item);
                                if (!result)
                                {
                                    Console.WriteLine($"Failed to update {item.Id}");
                                }
                            }
                            else
                            {
                                var result = await Remote.PostService.Create(item);
                                if (result is null)
                                {
                                    Console.WriteLine($"Failed to create {item.Id}");
                                }
                            }
                            remoteIds.Remove(item.Id);
                        }
                        if (full)
                        {
                            foreach(var v in remoteIds)
                            {
                                await Remote.PostService.Delete(v);
                            }
                        }
                    }
                    break;
                }
            }
            else
            {
                throw new Exception("No remote");
            }
        }
    }
}

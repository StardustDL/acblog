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

        public WorkspaceOption Option { get; }

        public DB DB { get; }

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

        public async Task Push(string name = "")
        {
            if (string.IsNullOrEmpty(name))
                name = Option.CurrentRemote;

            await Connect(name);

            var remote = Option.Remotes[name];
            switch (remote.Type)
            {
                case RemoteType.LocalFS:
                {
                    FSBuilder fsBuilder = new FSBuilder(remote.Uri);
                    fsBuilder.EnsureDirectoryEmpty();

                    List<Post> posts = new List<Post>();
                    foreach(var item in await Local.PostService.GetAllPosts())
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
                    throw new Exception("Not support pushing to remote fs");
                }
                case RemoteType.Api:
                {
                    throw new Exception("Not support pushing to api");
                }
            }
            Remote.PostService.Context.Token = remote.Token;

            Option.CurrentRemote = name;
            await SaveOption();
        }
    }
}

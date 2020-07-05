using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using AcBlog.Sdk;
using AcBlog.Sdk.Api;
using AcBlog.Sdk.FileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using StardustDL.Extensions.FileProviders.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Models
{
    public class Workspace
    {
        public const string OptionPath = "acblog.json";

        public const string DBPath = "db.json";

        public Workspace(IOptions<WorkspaceOption> option, IOptions<DB> db, IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
            BlogService = new AcBlog.Sdk.FileSystem.FileSystemBlogService(new PhysicalFileProvider(Environment.CurrentDirectory));
            Option = option.Value;
            DB = db.Value;
        }

        private WorkspaceOption Option { get; }

        private DB DB { get; }

        private IHttpClientFactory HttpClientFactory { get; }

        public IBlogService BlogService { get; private set; }

        public Task Connect(string name)
        {
            if (Option.Remotes.TryGetValue(name, out var remote))
            {
                switch (remote.Type)
                {
                    case RemoteType.LocalFS:
                        BlogService = new FileSystemBlogService(
                            new PhysicalFileProvider(remote.Uri));
                        break;
                    case RemoteType.RemoteFS:
                    {
                        var client = HttpClientFactory.CreateClient();
                        client.BaseAddress = new Uri(remote.Uri);
                        BlogService = new FileSystemBlogService(
                            new HttpFileProvider(client));
                    }
                    break;
                    case RemoteType.Api:
                    {
                        var client = HttpClientFactory.CreateClient();
                        client.BaseAddress = new Uri(remote.Uri);
                        BlogService = new ApiBlogService(client);
                    }
                    break;
                }
                BlogService.PostService.Context.Token = remote.Token;
            }
            else
            {
                throw new Exception("No remote");
            }
            return Task.CompletedTask;
        }
    }
}

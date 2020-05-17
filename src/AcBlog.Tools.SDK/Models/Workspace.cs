using AcBlog.Data.Repositories;
using AcBlog.SDK;
using AcBlog.SDK.API;
using AcBlog.SDK.StaticFile;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Tools.SDK.Models
{
    public class Workspace
    {
        private Workspace(DirectoryInfo root)
        {
            Root = root;
            DbRoot = new DirectoryInfo(Path.Join(Root.FullName, ".acblog"));
        }

        public WorkspaceConfiguration Configuration { get; private set; } = new WorkspaceConfiguration();

        public DirectoryInfo Root { get; }

        public DirectoryInfo DbRoot { get; }

        public IBlogService? Local { get; private set; }

        public bool HasInitialized => Local != null;

        public IBlogService? Remote { get; private set; }

        public Task Connect(HttpClient httpClient)
        {
            if (Configuration.Remote == null)
                throw new Exception("No remote configured.");
            httpClient.BaseAddress = Configuration.Remote.Uri;
            if (Configuration.Remote.IsStatic)
            {
                Remote = new HttpStaticFileBlogService(Configuration.Remote.Uri.LocalPath, httpClient);
            }
            else
            {
                Remote = new HttpApiBlogService(httpClient);
            }
            return Task.CompletedTask;
        }

        public Task Login()
        {
            if (Remote == null)
                throw new Exception("Please connect first.");
            Remote.PostService.Context ??= new RepositoryAccessContext();
            Remote.KeywordService.Context ??= new RepositoryAccessContext();
            Remote.CategoryService.Context ??= new RepositoryAccessContext();
            Remote.UserService.Context ??= new RepositoryAccessContext();
            Remote.PostService.Context.Token = Configuration.Token;
            Remote.KeywordService.Context.Token = Configuration.Token;
            Remote.CategoryService.Context.Token = Configuration.Token;
            Remote.UserService.Context.Token = Configuration.Token;
            return Task.CompletedTask;
        }

        public static async Task<Workspace> Load(DirectoryInfo root)
        {
            var result = new Workspace(root);
            if (result.DbRoot.Exists)
            {
                var fileinfo = new FileInfo(Path.Join(result.DbRoot.FullName, "config.json"));
                if (fileinfo.Exists)
                {
                    using var fs = fileinfo.Open(FileMode.Open, FileAccess.Read);
                    result.Configuration = await JsonSerializer.DeserializeAsync<WorkspaceConfiguration>(fs);
                }
            }
            return result;
        }

        public async Task Save()
        {
            DbRoot.Refresh();
            if (!DbRoot.Exists)
            {
                DbRoot.Create();
                DbRoot.Refresh();
            }

            var fileinfo = new FileInfo(Path.Join(DbRoot.FullName, "config.json"));
            using var fs = fileinfo.Open(FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(fs, Configuration);
        }
    }
}

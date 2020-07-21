using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using AcBlog.Tools.Sdk.Models.Text;
using Microsoft.Extensions.FileProviders;
using StardustDL.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Repositories
{
    internal abstract class RecordFSRepo<T, TQuery, TMeta> : RecordFSRepository<T, string, TQuery> where T : class, IHasId<string> where TQuery : QueryRequest, new() where TMeta : MetadataBase<T>, new()
    {
        public RecordFSRepo(string rootPath) : base(rootPath)
        {
            FSStaticBuilder.EnsureDirectoryExists(rootPath);
            FileProvider = new PhysicalFileProvider(rootPath).AsFileProvider();
        }

        Lazy<RepositoryStatus> _status = new Lazy<RepositoryStatus>(() =>
        {
            return new RepositoryStatus
            {
                CanRead = true,
                CanWrite = true
            };
        });

        protected virtual string GetPath(string id) => Path.Join(RootPath, $"{id}.md");

        public override Task<IEnumerable<string>> All(CancellationToken cancellationToken = default)
        {
            List<string> result = new List<string>();
            foreach (var file in Directory.EnumerateFiles(RootPath, "*.md", SearchOption.AllDirectories))
            {
                var name = Path.GetRelativePath(RootPath, file);
                result.Add(name.Substring(0, name.Length - 3).Replace('\\', '/'));
            }
            return Task.FromResult<IEnumerable<string>>(result);
        }

        protected abstract Task<T> CreateExistedItem(string id, TMeta metadata, string content);

        protected abstract Task<(TMeta, string)> CreateNewItem(T value);

        public override async Task<T?> Get(string id, CancellationToken cancellationToken = default)
        {
            string path = GetPath(id);
            using var fs = File.OpenRead(path);
            using var sr = new StreamReader(fs);
            var src = await sr.ReadToEndAsync().ConfigureAwait(false);
            var (metadata, content) = ObjectTextual.Parse<TMeta>(src);

            return await CreateExistedItem(id, metadata, content).ConfigureAwait(false);
        }

        public override async Task<string?> Create(T value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(value.Id))
            {
                value.Id = Guid.NewGuid().ToString();
            }
            var (metadata, content) = await CreateNewItem(value);
            string result = ObjectTextual.Format(metadata, content);
            await File.WriteAllTextAsync(GetPath(value.Id), result, System.Text.Encoding.UTF8, cancellationToken);
            return value.Id;
        }

        public override Task<bool> Delete(string id, CancellationToken cancellationToken = default)
        {
            string path = GetPath(id);
            if (File.Exists(path))
            {
                File.Delete(path);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public override Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(File.Exists(GetPath(id)));
        }

        public override async Task<bool> Update(T value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(value.Id))
            {
                throw new Exception("No item id");
            }
            var (metadata, content) = await CreateNewItem(value);
            string result = ObjectTextual.Format(metadata, content);
            await File.WriteAllTextAsync(GetPath(value.Id), result, System.Text.Encoding.UTF8, cancellationToken);
            return true;
        }

        public override Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => Task.FromResult(_status.Value);
    }
}

using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using AcBlog.Data.Repositories.FileSystem.Readers;
using AcBlog.Tools.Sdk.Models.Text;
using StardustDL.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Repositories
{
    internal class PostFSRepo : RecordFSRepository<Post, string, PostQueryRequest>, IPostRepository
    {
        public PostFSRepo(string absPath, string rootPath, IFileProvider fileProvider, IProtector<Document> protector) : base(rootPath, fileProvider)
        {
            Protector = protector;
            AbsolutePath = absPath;
        }

        Lazy<RepositoryStatus> _status = new Lazy<RepositoryStatus>(() =>
        {
            return new RepositoryStatus
            {
                CanRead = true,
                CanWrite = true
            };
        });

        public IProtector<Document> Protector { get; }

        public string AbsolutePath { get; }

        protected virtual string GetPath(string id) => Path.Join(RootPath, $"{id}.md");

        protected virtual string GetAbsolutePath(string id) => Path.Join(AbsolutePath, $"{id}.md");

        public override async Task<IEnumerable<string>> All(CancellationToken cancellationToken = default)
        {
            List<string> result = new List<string>();
            await foreach (var file in (await FileProvider.GetDirectoryContents(RootPath)).Children())
            {
                if (file.IsDirectory || !file.Name.EndsWith(".md"))
                    continue;
                result.Add(Path.GetFileNameWithoutExtension(file.Name));
            }
            return result.AsEnumerable();
        }

        public override async Task<Post?> Get(string id, CancellationToken cancellationToken = default)
        {
            using var fs = await (await FileProvider.GetFileInfo(GetPath(id))).CreateReadStream();
            using var sr = new StreamReader(fs);
            var src = await sr.ReadToEndAsync();
            var (metadata, content) = ObjectTextual.Parse<PostMetadata>(src);

            if (string.IsNullOrEmpty(metadata.id))
                metadata.id = id;
            if (string.IsNullOrEmpty(metadata.creationTime))
            {
                metadata.creationTime = File.GetCreationTime(GetAbsolutePath(id)).ToString();
            }
            if (string.IsNullOrEmpty(metadata.modificationTime))
            {
                metadata.modificationTime = File.GetLastWriteTime(GetAbsolutePath(id)).ToString();
            }

            Post result = new Post();
            metadata.ApplyTo(result);
            result.Content = new Document
            {
                Raw = content
            };

            if (!string.IsNullOrEmpty(metadata.password))
            {
                result.Content = await Protector.Protect(result.Content, new ProtectionKey
                {
                    Password = metadata.password
                });
            }

            return result;
        }

        public override Task<QueryResponse<string>> Query(PostQueryRequest query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override async Task<string?> Create(Post value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(value.Id))
            {
                value.Id = Guid.NewGuid().ToString();
            }
            PostMetadata metadata = new PostMetadata(value);
            string result = ObjectTextual.Format(metadata, value.Content.Raw);
            await File.WriteAllTextAsync(GetAbsolutePath(value.Id), result, System.Text.Encoding.UTF8, cancellationToken);
            return value.Id;
        }

        public override Task<bool> Delete(string id, CancellationToken cancellationToken = default)
        {
            string path = GetAbsolutePath(id);
            if (File.Exists(path))
            {
                File.Delete(path);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public override async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            return await (await FileProvider.GetFileInfo(GetPath(id))).Exists();
        }

        public override async Task<bool> Update(Post value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(value.Id))
            {
                throw new Exception("No post id");
            }
            PostMetadata metadata = new PostMetadata(value);
            string result = ObjectTextual.Format(metadata, value.Content.Raw);
            await File.WriteAllTextAsync(GetAbsolutePath(value.Id), result, System.Text.Encoding.UTF8, cancellationToken);
            return true;
        }

        public override Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => Task.FromResult(_status.Value);
    }
}

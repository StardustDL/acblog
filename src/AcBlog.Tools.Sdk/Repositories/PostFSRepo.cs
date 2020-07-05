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
        public PostFSRepo(string rootPath, IFileProvider fileProvider, IProtector<Document> protector) : base(rootPath, fileProvider)
        {
            Protector = protector;
        }

        public IProtector<Document> Protector { get; }

        protected virtual string GetPath(string id)
        {
            return Path.Join(RootPath, $"{id}.md");
        }

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

            if (string.IsNullOrEmpty(result.Id))
                result.Id = id;

            return result;
        }

        public override Task<QueryResponse<string>> Query(PostQueryRequest query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<string?> Create(Post value, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public override Task<bool> Delete(string id, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public override async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            return await (await FileProvider.GetFileInfo(GetPath(id))).Exists();
        }

        public override Task<bool> Update(Post value, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public override Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}

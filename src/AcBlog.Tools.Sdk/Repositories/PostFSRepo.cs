using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using AcBlog.Data.Repositories.FileSystem.Readers;
using AcBlog.Tools.Sdk.Models.Text;
using Microsoft.Extensions.FileProviders;
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
        public PostFSRepo(string rootPath, IFileProvider fileProvider) : base(rootPath, fileProvider)
        {
        }

        protected virtual string GetPath(string id)
        {
            return Path.Join(RootPath, $"{id}.md");
        }

        public override Task<IEnumerable<string>> All(CancellationToken cancellationToken = default)
        {
            IEnumerable<string> getAll()
            {
                foreach (var file in FileProvider.GetDirectoryContents(RootPath))
                {
                    if (file.IsDirectory || !file.Name.EndsWith(".md"))
                        continue;
                    yield return Path.GetFileNameWithoutExtension(file.Name);
                }
            }
            return Task.FromResult(getAll());
        }

        public override async Task<Post?> Get(string id, CancellationToken cancellationToken = default)
        {
            using var fs = FileProvider.GetFileInfo(GetPath(id)).CreateReadStream();
            using var sr = new StreamReader(fs);
            var src = await sr.ReadToEndAsync();
            var (metadata, content) = ObjectTextual.Parse<PostMetadata>(src);

            Post result = new Post();
            metadata.ApplyTo(result);
            result.Content = new Document
            {
                Raw = content
            };

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

        public override Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(FileProvider.GetFileInfo(GetPath(id)).Exists);
        }

        public override Task<bool> Update(Post value, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public override Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}

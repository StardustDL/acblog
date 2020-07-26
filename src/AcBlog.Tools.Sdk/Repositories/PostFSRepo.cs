using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem;
using AcBlog.Data.Repositories.FileSystem.Readers;
using AcBlog.Tools.Sdk.Models.Text;
using Microsoft.Extensions.FileProviders;
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
    internal class PostFSRepo : RecordFSRepo<Post, PostQueryRequest, PostMetadata>, IPostRepository
    {
        public PostFSRepo(string rootPath, IProtector<Document> protector) : base(rootPath)
        {
            Protector = protector;
        }

        public IProtector<Document> Protector { get; }

        public override Task<QueryResponse<string>> Query(PostQueryRequest query, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new QueryResponse<string>(Array.Empty<string>()));
        }

        public Task<CategoryTree> GetCategories(CancellationToken cancellationToken = default) => Task.FromResult(new CategoryTree());

        public Task<KeywordCollection> GetKeywords(CancellationToken cancellationToken = default) => Task.FromResult(new KeywordCollection());

        protected override async Task<Post> CreateExistedItem(string id, PostMetadata metadata, string content)
        {
            string path = GetPath(id);
            if (string.IsNullOrEmpty(metadata.id))
                metadata.id = id;
            if (string.IsNullOrEmpty(metadata.creationTime))
            {
                metadata.creationTime = System.IO.File.GetCreationTime(path).ToString();
            }
            if (string.IsNullOrEmpty(metadata.modificationTime))
            {
                metadata.modificationTime = System.IO.File.GetLastWriteTime(path).ToString();
            }
            if (metadata.category.Length == 0)
            {
                var relpath = Path.GetDirectoryName(id)?.Replace("\\", "/");
                var items = relpath?.Split("/", StringSplitOptions.RemoveEmptyEntries);
                metadata.category = items ?? Array.Empty<string>();
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

        protected override Task<(PostMetadata, string)> CreateNewItem(Post value)
        {
            return Task.FromResult((new PostMetadata(value), value.Content.Raw));
        }
    }
}

using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Tools.Sdk.Models.Text;
using System;
using System.IO;
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

        public Task<CategoryTree> GetCategories(CancellationToken cancellationToken = default) => Task.FromResult(new CategoryTree());

        public Task<KeywordCollection> GetKeywords(CancellationToken cancellationToken = default) => Task.FromResult(new KeywordCollection());

        public PostMetadata GetDefaultMetadata(string id)
        {
            string path = GetPath(id);
            PostMetadata metadata = new PostMetadata
            {
                id = id,
                creationTime = System.IO.File.GetCreationTime(path).ToString(),
                modificationTime = System.IO.File.GetLastWriteTime(path).ToString()
            };
            {
                var relpath = Path.GetDirectoryName(id)?.Replace("\\", "/");
                var items = relpath?.Split("/", StringSplitOptions.RemoveEmptyEntries);
                metadata.category = items ?? Array.Empty<string>();
            }
            return metadata;
        }

        protected override async Task<Post> CreateExistedItem(string id, PostMetadata metadata, string content)
        {
            var defaultMeta = GetDefaultMetadata(id);
            if (string.IsNullOrEmpty(metadata.id))
                metadata.id = defaultMeta.id;
            if (string.IsNullOrEmpty(metadata.creationTime))
            {
                metadata.creationTime = defaultMeta.creationTime;
            }
            if (string.IsNullOrEmpty(metadata.modificationTime))
            {
                metadata.modificationTime = defaultMeta.modificationTime;
            }
            if (!(metadata.category?.Length > 0))
            {
                metadata.category = defaultMeta.category;
            }

            Post result = new Post();
            metadata.ApplyTo(result);
            result.Content = new Document
            {
                Raw = content
            };

            if (!string.IsNullOrWhiteSpace(metadata.password))
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

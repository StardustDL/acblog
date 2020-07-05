using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem.Readers;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk.FileSystem
{
    internal class PostService : IPostService
    {
        PostFSReader Reader { get; }

        public RepositoryAccessContext Context { get => Reader.Context; set => Reader.Context = value; }

        public IBlogService BlogService { get; }

        public IProtector<Document> Protector { get; }

        public PostService(IBlogService blog, string rootPath, IFileProvider fileProvider)
        {
            BlogService = blog;
            Protector = new DocumentProtector();
            Reader = new PostFSReader(rootPath, fileProvider);
        }

        public Task<IEnumerable<string>> All(CancellationToken cancellationToken = default) => Reader.All(cancellationToken);

        public Task<string?> Create(Post value, CancellationToken cancellationToken = default) => Reader.Create(value, cancellationToken);

        public Task<bool> Delete(string id, CancellationToken cancellationToken = default) => Reader.Delete(id, cancellationToken);

        public Task<bool> Exists(string id, CancellationToken cancellationToken = default) => Reader.Exists(id, cancellationToken);

        public Task<Post?> Get(string id, CancellationToken cancellationToken = default) => Reader.Get(id, cancellationToken);

        public Task<bool> Update(Post value, CancellationToken cancellationToken = default) => Reader.Update(value, cancellationToken);

        public Task<QueryResponse<string>> Query(PostQueryRequest query, CancellationToken cancellationToken = default) => Reader.Query(query, cancellationToken);

        public Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => Reader.GetStatus(cancellationToken);
    }
}

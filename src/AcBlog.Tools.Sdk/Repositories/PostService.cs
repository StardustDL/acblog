using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Sdk;
using AcBlog.Tools.Sdk.Repositories;
using StardustDL.Extensions.FileProviders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk
{
    internal class PostService : IPostService
    {
        PostFSRepo Repo { get; }

        public RepositoryAccessContext Context { get => Repo.Context; set => Repo.Context = value; }

        public IBlogService BlogService { get; }

        public IProtector<Document> Protector { get; }

        public PostService(IBlogService blog, string rootPath)
        {
            BlogService = blog;
            Protector = new DocumentProtector();
            Repo = new PostFSRepo(rootPath, Protector);
        }

        public Task<IEnumerable<string>> All(CancellationToken cancellationToken = default) => Repo.All(cancellationToken);

        public Task<string?> Create(Post value, CancellationToken cancellationToken = default) => Repo.Create(value, cancellationToken);

        public Task<bool> Delete(string id, CancellationToken cancellationToken = default) => Repo.Delete(id, cancellationToken);

        public Task<bool> Exists(string id, CancellationToken cancellationToken = default) => Repo.Exists(id, cancellationToken);

        public Task<Post?> Get(string id, CancellationToken cancellationToken = default) => Repo.Get(id, cancellationToken);

        public Task<bool> Update(Post value, CancellationToken cancellationToken = default) => Repo.Update(value, cancellationToken);

        public Task<QueryResponse<string>> Query(PostQueryRequest query, CancellationToken cancellationToken = default) => Repo.Query(query, cancellationToken);

        public Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => Repo.GetStatus(cancellationToken);

        public Task<CategoryTree> GetCategories(CancellationToken cancellationToken = default) => Repo.GetCategories(cancellationToken);

        public Task<KeywordCollection> GetKeywords(CancellationToken cancellationToken = default) => Repo.GetKeywords(cancellationToken);
    }
}

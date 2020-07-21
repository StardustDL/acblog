using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Sdk;
using AcBlog.Tools.Sdk.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk
{
    internal class PageService : IPageService
    {
        PageFSRepo Repo { get; }

        public RepositoryAccessContext Context { get => Repo.Context; set => Repo.Context = value; }

        public IBlogService BlogService { get; }

        public PageService(IBlogService blog, string rootPath)
        {
            BlogService = blog;
            Repo = new PageFSRepo(rootPath);
        }

        public Task<IEnumerable<string>> All(CancellationToken cancellationToken = default) => Repo.All(cancellationToken);

        public Task<string?> Create(Page value, CancellationToken cancellationToken = default) => Repo.Create(value, cancellationToken);

        public Task<bool> Delete(string id, CancellationToken cancellationToken = default) => Repo.Delete(id, cancellationToken);

        public Task<bool> Exists(string id, CancellationToken cancellationToken = default) => Repo.Exists(id, cancellationToken);

        public Task<Page?> Get(string id, CancellationToken cancellationToken = default) => Repo.Get(id, cancellationToken);

        public Task<bool> Update(Page value, CancellationToken cancellationToken = default) => Repo.Update(value, cancellationToken);

        public Task<QueryResponse<string>> Query(PageQueryRequest query, CancellationToken cancellationToken = default) => Repo.Query(query, cancellationToken);

        public Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => Repo.GetStatus(cancellationToken);
    }
}

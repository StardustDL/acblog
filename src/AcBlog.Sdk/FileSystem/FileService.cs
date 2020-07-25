using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem.Readers;
using StardustDL.Extensions.FileProviders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk.FileSystem
{
    internal class FileService : IFileService
    {
        FileFSReader Repo { get; }

        public RepositoryAccessContext Context { get => Repo.Context; set => Repo.Context = value; }

        public IBlogService BlogService { get; }

        public FileService(IBlogService blog, string rootPath, IFileProvider fileProvider)
        {
            BlogService = blog;
            Repo = new FileFSReader(rootPath, fileProvider);
        }

        public Task<IEnumerable<string>> All(CancellationToken cancellationToken = default) => Repo.All(cancellationToken);

        public Task<string?> Create(File value, CancellationToken cancellationToken = default) => Repo.Create(value, cancellationToken);

        public Task<bool> Delete(string id, CancellationToken cancellationToken = default) => Repo.Delete(id, cancellationToken);

        public Task<bool> Exists(string id, CancellationToken cancellationToken = default) => Repo.Exists(id, cancellationToken);

        public Task<File?> Get(string id, CancellationToken cancellationToken = default) => Repo.Get(id, cancellationToken);

        public Task<bool> Update(File value, CancellationToken cancellationToken = default) => Repo.Update(value, cancellationToken);

        public Task<QueryResponse<string>> Query(FileQueryRequest query, CancellationToken cancellationToken = default) => Repo.Query(query, cancellationToken);

        public Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => Repo.GetStatus(cancellationToken);
    }
}

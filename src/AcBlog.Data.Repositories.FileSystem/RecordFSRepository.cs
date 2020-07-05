using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using StardustDL.Extensions.FileProviders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem
{
    public abstract class RecordFSRepository<T, TId, TQuery> : IRecordRepository<T, TId, TQuery> where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
    {
        protected RecordFSRepository(string rootPath, IFileProvider fileProvider)
        {
            RootPath = rootPath;
            FileProvider = fileProvider;
        }

        public string RootPath { get; }

        protected IFileProvider FileProvider { get; }

        public RepositoryAccessContext Context { get; set; } = new RepositoryAccessContext();

        public abstract Task<IEnumerable<TId>> All(CancellationToken cancellationToken = default);

        public abstract Task<QueryResponse<TId>> Query(TQuery query, CancellationToken cancellationToken = default);

        public abstract Task<TId?> Create(T value, CancellationToken cancellationToken = default);

        public abstract Task<bool> Delete(TId id, CancellationToken cancellationToken = default);

        public abstract Task<bool> Exists(TId id, CancellationToken cancellationToken = default);

        public abstract Task<T?> Get(TId id, CancellationToken cancellationToken = default);

        public abstract Task<bool> Update(T value, CancellationToken cancellationToken = default);

        public abstract Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default);
    }
}

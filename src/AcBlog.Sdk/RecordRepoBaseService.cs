using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk
{
    public abstract class RecordRepoBaseService<T, TId, TQuery, TRepo> : IRecordRepository<T, TId, TQuery> where TRepo : IRecordRepository<T, TId, TQuery> where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
    {
        protected RecordRepoBaseService(IBlogService blogService, TRepo repository)
        {
            Repository = repository;
            BlogService = blogService;
        }

        public TRepo Repository { get; }

        public IBlogService BlogService { get; }

        public RepositoryAccessContext Context { get => Repository.Context; set => Repository.Context = value; }

        public IAsyncEnumerable<TId> All(CancellationToken cancellationToken = default) => Repository.All(cancellationToken);

        public Task<TId?> Create(T value, CancellationToken cancellationToken = default) => Repository.Create(value, cancellationToken);

        public Task<bool> Delete(TId id, CancellationToken cancellationToken = default) => Repository.Delete(id, cancellationToken);

        public Task<bool> Exists(TId id, CancellationToken cancellationToken = default) => Repository.Exists(id, cancellationToken);

        public Task<T?> Get(TId id, CancellationToken cancellationToken = default) => Repository.Get(id, cancellationToken);

        public Task<bool> Update(T value, CancellationToken cancellationToken = default) => Repository.Update(value, cancellationToken);

        public IAsyncEnumerable<TId> Query(TQuery query, CancellationToken cancellationToken = default) => Repository.Query(query, cancellationToken);

        public Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => Repository.GetStatus(cancellationToken);

        public Task<QueryStatistic> Statistic(TQuery query, CancellationToken cancellationToken = default) => Repository.Statistic(query, cancellationToken);
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.SqlServer
{
    public abstract class RecordDBRepository<T, TId, TQuery, TRaw> : IRecordRepository<T, TId, TQuery> where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new() where TRaw : class, IHasId<TId>
    {
        public RecordDBRepository(BlogDataContext dataSource) => DataSource = dataSource;

        private static readonly Lazy<RepositoryStatus> _status = new Lazy<RepositoryStatus>(new RepositoryStatus
        {
            CanRead = true,
            CanWrite = true,
        });

        protected BlogDataContext DataSource { get; set; }

        protected abstract DbSet<TRaw> DbSet { get; }

        protected abstract TRaw ToRaw(T item);

        protected abstract T ToModel(TRaw item);

        protected abstract void ApplyChanges(TRaw target, TRaw value);

        public virtual RepositoryAccessContext Context { get; set; } = new RepositoryAccessContext();

        public virtual IAsyncEnumerable<TId> All(CancellationToken cancellationToken = default) => DbSet.AsQueryable().Select(x => x.Id).AsAsyncEnumerable();

        public virtual async Task<TId> Create(T value, CancellationToken cancellationToken = default)
        {
            DbSet.Add(ToRaw(value));
            await DataSource.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return value.Id;
        }

        public virtual async Task<bool> Delete(TId id, CancellationToken cancellationToken = default)
        {
            var item = await DbSet.FindAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
            if (item is null)
                return false;
            DbSet.Remove(item);
            await DataSource.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }

        public virtual async Task<bool> Exists(TId id, CancellationToken cancellationToken = default)
        {
            var item = await DbSet.FindAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
            return item is not null;
        }

        public virtual async Task<T> Get(TId id, CancellationToken cancellationToken = default)
        {
            var item = await DbSet.FindAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
            return ToModel(item);
        }

        protected virtual IQueryable<TId> InnerQuery(TQuery query)
        {
            var qr = DbSet.AsQueryable();

            qr = qr.OrderBy(x => x.Id);

            if (query.Pagination is not null)
            {
                qr = qr.Skip(query.Pagination.Offset).Take(query.Pagination.PageSize);
            }

            return qr.Select(x => x.Id);
        }

        public virtual IAsyncEnumerable<TId> Query(TQuery query, CancellationToken cancellationToken = default)
        {
            return InnerQuery(query).AsAsyncEnumerable();
        }

        public virtual async Task<QueryStatistic> Statistic(TQuery query, CancellationToken cancellationToken = default)
        {
            query.Pagination = null;
            var count = await InnerQuery(query).CountAsync(cancellationToken).ConfigureAwait(false);
            return new QueryStatistic
            {
                Count = count
            };
        }

        public virtual async Task<bool> Update(T value, CancellationToken cancellationToken = default)
        {
            var to = ToRaw(value);

            var item = await DbSet.FindAsync(new object[] { to.Id }, cancellationToken).ConfigureAwait(false);
            if (item is null)
                return false;

            ApplyChanges(item, to);

            DbSet.Update(item);
            await DataSource.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }

        public virtual Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => Task.FromResult(_status.Value);
    }
}

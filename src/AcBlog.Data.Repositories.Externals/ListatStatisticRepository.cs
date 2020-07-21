using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using Listat;
using Loment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Externals
{
    public class ListatStatisticRepository : IStatisticRepository
    {
        public ListatStatisticRepository(IListatService service)
        {
            Service = service;
        }

        Lazy<RepositoryStatus> Status = new Lazy<RepositoryStatus>(new RepositoryStatus
        {
            CanRead = true,
            CanWrite = true,
        });

        public RepositoryAccessContext Context { get; set; } = new RepositoryAccessContext();

        public IListatService Service { get; }

        public async Task<IEnumerable<string>> All(CancellationToken cancellationToken = default)
        {
            var items = await Service.Query(new Listat.Models.StatisticQuery
            {
                Offset = 0,
                Limit = int.MaxValue - 1,
            }, cancellationToken).ConfigureAwait(false);

            return items.Select(x => x.Id);
        }

        public Task<string?> Create(Statistic value, CancellationToken cancellationToken = default)
        {
            return Service.Create(new Listat.Models.Statistic
            {
                CreationTime = value.CreationTime,
                Payload = value.Payload,
                Id = value.Id,
                Category = value.Category,
                ModificationTime = value.ModificationTime,
                Uri = value.Uri
            }, cancellationToken);
        }

        public Task<bool> Delete(string id, CancellationToken cancellationToken = default)
        {
            return Service.Delete(id, cancellationToken);
        }

        public async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await Get(id, cancellationToken).ConfigureAwait(false);
                return result != null;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Statistic?> Get(string id, CancellationToken cancellationToken = default)
        {
            var value = await Service.Get(id, cancellationToken).ConfigureAwait(false);
            if (value is null)
                return null;
            return new Statistic
            {
                CreationTime = value.CreationTime,
                Payload = value.Payload,
                Id = value.Id,
                Category = value.Category,
                ModificationTime = value.ModificationTime,
                Uri = value.Uri
            };
        }

        public Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => Task.FromResult(Status.Value);

        public async Task<QueryResponse<string>> Query(StatisticQueryRequest query, CancellationToken cancellationToken = default)
        {
            var pagination = query.Pagination ?? new Pagination();

            var items = await Service.Query(new Listat.Models.StatisticQuery
            {
                Category = query.Category,
                Payload = query.Payload,
                Uri = query.Uri,
                Offset = pagination.Offset,
                Limit = pagination.PageSize,
            }, cancellationToken).ConfigureAwait(false);

            return new QueryResponse<string>(items.Select(x => x.Id), pagination);
        }

        public Task<bool> Update(Statistic value, CancellationToken cancellationToken = default)
        {
            return Service.Update(new Listat.Models.Statistic
            {
                CreationTime = value.CreationTime,
                Payload = value.Payload,
                Id = value.Id,
                Category = value.Category,
                ModificationTime = value.ModificationTime,
                Uri = value.Uri
            }, cancellationToken);
        }

        public Task<long> Count(StatisticQueryRequest query, CancellationToken cancellationToken = default)
        {
            return Service.Count(new Listat.Models.StatisticQuery
            {
                Category = query.Category,
                Payload = query.Payload,
                Uri = query.Uri,
            }, cancellationToken);
        }
    }
}

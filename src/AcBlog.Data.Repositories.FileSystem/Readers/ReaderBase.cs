using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class ReaderBase<T, TId, TQuery> : IRecordRepository<T, TId, TQuery> where TId : class where T : class, IHasId<TId>
    {
        protected ReaderBase(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        protected abstract string GetPath(TId id);

        protected abstract Task<Stream> GetFileReadStream(string path, CancellationToken cancellationToken = default);

        protected async Task EnsurePagingConfig(PagingPath paging, CancellationToken cancellationToken = default)
        {
            if (paging.Config != null)
                return;
            using var fs = await GetFileReadStream(paging.ConfigPath);
            paging.Config = await JsonSerializer.DeserializeAsync<PagingConfig>(fs, cancellationToken: cancellationToken);
        }

        protected async Task<IList<string>> GetPagingResult(PagingPath paging, Pagination pagination, CancellationToken cancellationToken = default)
        {
            IList<string> result = Array.Empty<string>();

            var path = paging.GetPagePath(pagination);
            if (path != null)
            {
                using var fs = await GetFileReadStream(path, cancellationToken);
                result = await JsonSerializer.DeserializeAsync<IList<string>>(fs, cancellationToken: cancellationToken);
            }

            return result;
        }

        public RepositoryAccessContext? Context { get; set; }

        public abstract Task<IEnumerable<TId>> All(CancellationToken cancellationToken = default);

        public abstract Task<QueryResponse<TId>> Query(TQuery query, CancellationToken cancellationToken = default);

        public Task<bool> CanRead(CancellationToken cancellationToken = default) => Task.FromResult(true);

        public Task<bool> CanWrite(CancellationToken cancellationToken = default) => Task.FromResult(false);

        public Task<TId?> Create(T value, CancellationToken cancellationToken = default) => Task.FromResult<TId?>(null);

        public Task<bool> Delete(TId id, CancellationToken cancellationToken = default) => Task.FromResult(false);

        public abstract Task<bool> Exists(TId id, CancellationToken cancellationToken = default);

        public virtual async Task<T?> Get(TId id, CancellationToken cancellationToken = default)
        {
            using var fs = await GetFileReadStream(GetPath(id), cancellationToken);
            var result = await JsonSerializer.DeserializeAsync<T?>(fs, cancellationToken: cancellationToken);
            if (result != null)
                result.Id = id;
            return result;
        }

        public Task<bool> Update(T value, CancellationToken cancellationToken = default) => Task.FromResult(false);
    }
}

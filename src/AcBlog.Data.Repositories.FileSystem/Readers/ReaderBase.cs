using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class RecordFSReader<T, TId, TQuery> : IRecordRepository<T, TId, TQuery> where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
    {
        protected RecordFSReader(string rootPath, IFileProvider fileProvider)
        {
            RootPath = rootPath;
            FileProvider = fileProvider;
        }

        public string RootPath { get; }

        protected IFileProvider FileProvider { get; }

        Lazy<RepositoryStatus> Status = new Lazy<RepositoryStatus>(new RepositoryStatus
        {
            CanRead = true,
            CanWrite = false,
        });

        protected virtual string GetPath(TId id) => Path.Join(RootPath, $"{id}.json").Replace("\\", "/");

        protected async Task EnsurePagingConfig(PagingPath paging, CancellationToken cancellationToken = default)
        {
            if (paging.Config != null)
                return;
            using var fs = FileProvider.GetFileInfo(paging.ConfigPath).CreateReadStream();
            paging.Config = await JsonSerializer.DeserializeAsync<PagingConfig>(fs, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task<IList<TId>> GetPagingResult(PagingPath paging, Pagination pagination, CancellationToken cancellationToken = default)
        {
            IList<TId> result = Array.Empty<TId>();

            var path = paging.GetPagePath(pagination);
            if (path != null)
            {
                using var fs = FileProvider.GetFileInfo(path).CreateReadStream();
                result = await JsonSerializer.DeserializeAsync<IList<TId>>(fs, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }

            return result;
        }

        public RepositoryAccessContext? Context { get; set; }

        public virtual async Task<IEnumerable<TId>> All(CancellationToken cancellationToken = default)
        {
            List<TId> result = new List<TId>();
            TQuery pq = new TQuery();
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var req = await Query(pq, cancellationToken).ConfigureAwait(false);
                result.AddRange(req.Results);
                if (!req.CurrentPage.HasNextPage)
                    break;
                pq.Pagination = req.CurrentPage.NextPage();
            }
            return result;
        }

        public virtual async Task<QueryResponse<TId>> Query(TQuery query, CancellationToken cancellationToken = default)
        {
            query.Pagination ??= new Pagination();

            PagingPath? paging = null;

            paging ??= new PagingPath(Path.Join(RootPath, "pages"));

            await EnsurePagingConfig(paging, cancellationToken).ConfigureAwait(false);
            paging.FillPagination(query.Pagination);

            var res = new QueryResponse<TId>(
                await GetPagingResult(paging, query.Pagination, cancellationToken).ConfigureAwait(false), 
                query.Pagination);
            return res;
        }

        public Task<TId?> Create(T value, CancellationToken cancellationToken = default) => Task.FromResult<TId?>(null);

        public Task<bool> Delete(TId id, CancellationToken cancellationToken = default) => Task.FromResult(false);

        public abstract Task<bool> Exists(TId id, CancellationToken cancellationToken = default);

        public virtual async Task<T?> Get(TId id, CancellationToken cancellationToken = default)
        {
            using var fs = FileProvider.GetFileInfo(GetPath(id)).CreateReadStream();
            var result = await JsonSerializer.DeserializeAsync<T?>(fs, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            if (result != null)
                result.Id = id;
            return result;
        }

        public Task<bool> Update(T value, CancellationToken cancellationToken = default) => Task.FromResult(false);

        public Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => Task.FromResult(Status.Value);
    }
}

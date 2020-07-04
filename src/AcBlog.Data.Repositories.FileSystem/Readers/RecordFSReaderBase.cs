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
    public abstract class RecordFSReaderBase<T, TId, TQuery> : IRecordRepository<T, TId, TQuery> where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
    {
        protected RecordFSReaderBase(string rootPath, IFileProvider fileProvider)
        {
            RootPath = rootPath;
            FileProvider = fileProvider;
            PagingProvider = new PagingProvider<TId>(Path.Join(RootPath, "pages"), FileProvider);
        }

        public string RootPath { get; }

        protected IFileProvider FileProvider { get; }

        protected PagingProvider<TId> PagingProvider { get; }

        Lazy<RepositoryStatus> Status = new Lazy<RepositoryStatus>(new RepositoryStatus
        {
            CanRead = true,
            CanWrite = false,
        });

        protected virtual string GetPath(TId id) => Path.Join(RootPath, $"{id}.json");

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

        public virtual Task<QueryResponse<TId>> Query(TQuery query, CancellationToken cancellationToken = default)
        {
            query.Pagination ??= new Pagination();

            PagingProvider.FillPagination(query.Pagination);

            var res = new QueryResponse<TId>(
                PagingProvider.GetPaging(query.Pagination),
                query.Pagination);
            return Task.FromResult(res);
        }

        public Task<TId?> Create(T value, CancellationToken cancellationToken = default) => Task.FromResult<TId?>(null);

        public Task<bool> Delete(TId id, CancellationToken cancellationToken = default) => Task.FromResult(false);

        public virtual Task<bool> Exists(TId id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(FileProvider.GetFileInfo(GetPath(id)).Exists);
        }

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

using AcBlog.Data.Models.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class ReaderBase<T, TId> : IRecordRepository<T, TId> where TId : class where T : class
    {
        protected ReaderBase(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        protected abstract string GetPath(TId id);

        protected abstract Task<Stream> GetFileReadStream(string path);

        protected async Task EnsurePagingConfig(PagingPath paging)
        {
            if (paging.Config != null)
                return;
            using var fs = await GetFileReadStream(paging.ConfigPath);
            paging.Config = await JsonSerializer.DeserializeAsync<PagingConfig>(fs);
        }

        protected async Task<IList<string>> GetPagingResult(PagingPath paging, Pagination pagination)
        {
            IList<string> result = Array.Empty<string>();

            try
            {
                var path = paging.GetPagePath(pagination);
                if (path != null)
                {
                    using var fs = await GetFileReadStream(path);
                    result = await JsonSerializer.DeserializeAsync<IList<string>>(fs);
                }
            }
            catch { }

            return result;
        }

        public RepositoryAccessContext? Context { get; set; }

        public abstract Task<IEnumerable<string>> All();

        public Task<bool> CanRead() => Task.FromResult(true);

        public Task<bool> CanWrite() => Task.FromResult(false);

        public Task<TId?> Create(T value) => Task.FromResult<TId?>(null);

        public Task<bool> Delete(TId id) => Task.FromResult(false);

        public abstract Task<bool> Exists(TId id);

        public virtual async Task<T?> Get(TId id)
        {
            using var fs = await GetFileReadStream(GetPath(id));
            var result = await JsonSerializer.DeserializeAsync<T?>(fs);
            return result;
        }

        public Task<bool> Update(T value) => Task.FromResult(false);
    }
}

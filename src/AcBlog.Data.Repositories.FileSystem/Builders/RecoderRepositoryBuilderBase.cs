using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Builders
{
    public abstract class RecoderRepositoryBuilderBase<T, TId> where TId : class where T : class, IHasId<TId>
    {
        protected RecoderRepositoryBuilderBase(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        public PagingConfig PagingConfig { get; set; } = new PagingConfig();

        protected virtual async Task BuildData(IList<T> data)
        {
            foreach (var v in data)
            {
                await using var st = FSStaticBuilder.GetFileRewriteStream(Paths.GetFileById(RootPath, v.Id.ToString() ?? throw new NullReferenceException(nameof(v.Id))));
                await JsonSerializer.SerializeAsync(st, v).ConfigureAwait(false);
            }
        }

        protected virtual async Task BuildPaging(IList<T> data)
        {
            PagingProvider<string> paging = new PagingProvider<string>(Paths.GetPaginationRoot(RootPath));

            await paging.Build(data.Select(x => x.Id.ToString() ?? throw new NullReferenceException(nameof(x.Id))).ToArray(),
                PagingConfig).ConfigureAwait(false);
        }

        public virtual async Task Build(IList<T> data)
        {
            FSStaticBuilder.EnsureDirectoryEmpty(RootPath);

            await BuildData(data).ConfigureAwait(false);
            await BuildPaging(data).ConfigureAwait(false);
        }
    }
}

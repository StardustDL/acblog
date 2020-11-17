using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Builders
{
    public abstract class RecordFSBuilderBase<T, TId> where TId : class where T : RHasId<TId>
    {
        protected RecordFSBuilderBase(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        protected virtual async Task BuildDataIdList(IList<TId> dataIds, string rootPath)
        {
            {
                var stats = new QueryStatistic
                {
                    Count = dataIds.Count
                };
                await using var st = FSStaticBuilder.GetFileRewriteStream(Paths.GetStatisticFile(rootPath));
                await JsonSerializer.SerializeAsync(st, stats).ConfigureAwait(false);
            }

            {
                await using var st = FSStaticBuilder.GetFileRewriteStream(Paths.GetIdListFile(rootPath));
                await JsonSerializer.SerializeAsync(st, dataIds).ConfigureAwait(false);
            }
        }

        protected virtual async Task BuildData(IList<T> data)
        {
            List<TId> ids = new List<TId>();

            foreach (var v in data)
            {
                var id = v.Id?.ToString() ?? throw new NullReferenceException(nameof(v.Id));
                await using var st = FSStaticBuilder.GetFileRewriteStream(Paths.GetDataFile(RootPath, id));
                await JsonSerializer.SerializeAsync(st, v).ConfigureAwait(false);
                ids.Add(v.Id);
            }

            await BuildDataIdList(ids, Paths.GetConfigRoot(RootPath)).ConfigureAwait(false);
        }

        public virtual async Task Build(IList<T> data)
        {
            FSStaticBuilder.EnsureDirectoryEmpty(RootPath);
            FSStaticBuilder.EnsureDirectoryEmpty(Paths.GetDataRoot(RootPath));
            FSStaticBuilder.EnsureDirectoryEmpty(Paths.GetConfigRoot(RootPath));
            FSStaticBuilder.EnsureDirectoryEmpty(Paths.GetIndexRoot(RootPath));

            await BuildData(data).ConfigureAwait(false);
        }
    }
}

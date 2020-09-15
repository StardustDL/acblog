using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Builders
{
    public abstract class RecordRepositoryBuilderBase<T, TId> where TId : class where T : class, IHasId<TId>
    {
        protected RecordRepositoryBuilderBase(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        protected virtual async Task BuildData(IList<T> data)
        {
            List<string> ids = new List<string>();
            
            foreach (var v in data)
            {
                var id = v.Id.ToString() ?? throw new NullReferenceException(nameof(v.Id));
                await using var st = FSStaticBuilder.GetFileRewriteStream(Paths.GetDataFile(RootPath, id));
                await JsonSerializer.SerializeAsync(st, v).ConfigureAwait(false);
                ids.Add(id);
            }

            {
                var stats = new RepositoryStatistic
                {
                    Count = data.Count
                };
                await using var st = FSStaticBuilder.GetFileRewriteStream(Paths.GetStatisticFile(RootPath));
                await JsonSerializer.SerializeAsync(st, stats).ConfigureAwait(false);
            }

            {
                await using var st = FSStaticBuilder.GetFileRewriteStream(Paths.GetIdListFile(RootPath));
                await JsonSerializer.SerializeAsync(st, ids).ConfigureAwait(false);
            }
        }

        public virtual async Task Build(IList<T> data)
        {
            FSStaticBuilder.EnsureDirectoryEmpty(RootPath);
            FSStaticBuilder.EnsureDirectoryEmpty(Paths.GetDataRoot(RootPath));
            FSStaticBuilder.EnsureDirectoryEmpty(Paths.GetConfigRoot(RootPath));

            await BuildData(data).ConfigureAwait(false);
        }
    }
}

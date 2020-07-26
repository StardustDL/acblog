using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Extensions
{
    public static class RecordRepositoryExtensions
    {
        public static Task<T?[]> GetItems<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository, IEnumerable<TId> ids, CancellationToken cancellationToken = default) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            List<Task<T?>> posts = new List<Task<T?>>();
            foreach (var id in ids)
                posts.Add(repository.Get(id, cancellationToken));
            return Task.WhenAll(posts.ToArray());
        }

        public static async Task<T?[]> GetAllItems<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository, CancellationToken cancellationToken = default) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            return await repository.GetItems(await repository.All());
        }

        public static IEnumerable<T> IgnoreNull<T>(this IEnumerable<T?> raw) where T : class
        {
            foreach (var v in raw)
            {
                if (v is null)
                    continue;
                yield return v;
            }
        }
    }
}

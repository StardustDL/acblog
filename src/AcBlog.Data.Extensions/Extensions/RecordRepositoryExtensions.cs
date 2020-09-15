using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Data.Repositories.Searchers.Local;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AcBlog.Data.Extensions
{
    public static class RecordRepositoryExtensions
    {
        public static IAsyncEnumerable<T?> GetItems<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository, IAsyncEnumerable<TId> ids, CancellationToken cancellationToken = default) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            return ids.SelectAwaitWithCancellation(
                async (id, cancellationToken) => await repository.Get(id, cancellationToken));
        }

        public static IAsyncEnumerable<T?> GetAllItems<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository, [EnumeratorCancellation] CancellationToken cancellationToken = default) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            return GetItems(repository, repository.All(), cancellationToken);
        }

        public static async IAsyncEnumerable<T> IgnoreNull<T>(this IAsyncEnumerable<T?> collection)
        {
            await foreach (var item in collection)
            {
                if (item is not null)
                    yield return item;
            }
        }

        public static IAsyncEnumerable<T> Paging<T>(this IAsyncEnumerable<T> collection, Pagination? pagination)
        {
            if (pagination is null)
                return collection;
            return collection.Skip(pagination.Offset).Take(pagination.PageSize);
        }
    }
}

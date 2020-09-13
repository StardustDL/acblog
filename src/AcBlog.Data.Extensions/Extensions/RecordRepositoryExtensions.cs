using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Data.Repositories.Searchers.Local;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AcBlog.Data.Extensions
{
    public static class RecordRepositoryExtensions
    {
        public static async IAsyncEnumerable<T?> GetItems<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository, IEnumerable<TId> ids, [EnumeratorCancellation] CancellationToken cancellationToken = default) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            foreach (var id in ids)
            {
                yield return await repository.Get(id, cancellationToken);
            }
        }

        public static async IAsyncEnumerable<T?> GetAllItems<T, TId, TQuery>(this IRecordRepository<T, TId, TQuery> repository, [EnumeratorCancellation] CancellationToken cancellationToken = default) where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new()
        {
            await foreach (var item in repository.GetItems(await repository.All(cancellationToken), cancellationToken))
                yield return item;
        }

        public static async IAsyncEnumerable<T> IgnoreNull<T>(this IAsyncEnumerable<T?> collection)
        {
            await foreach (var item in collection)
            {
                if (item is not null)
                    yield return item;
            }
        }

        public static ICommentRepositorySearcher CreateLocalSearcher(this ICommentRepository repository)
        {
            return new LocalCommentRepositorySearcher(repository);
        }

        public static IStatisticRepositorySearcher CreateLocalSearcher(this IStatisticRepository repository)
        {
            return new LocalStatisticRepositorySearcher(repository);
        }

        public static ILayoutRepositorySearcher CreateLocalSearcher(this ILayoutRepository repository)
        {
            return new LocalLayoutRepositorySearcher(repository);
        }

        public static IPageRepositorySearcher CreateLocalSearcher(this IPageRepository repository)
        {
            return new LocalPageRepositorySearcher(repository);
        }

        public static IPostRepositorySearcher CreateLocalSearcher(this IPostRepository repository)
        {
            return new LocalPostRepositorySearcher(repository);
        }

        public static IFileRepositorySearcher CreateLocalSearcher(this IFileRepository repository)
        {
            return new LocalFileRepositorySearcher(repository);
        }
    }
}

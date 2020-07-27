using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Data.Repositories.Searchers.Local;
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

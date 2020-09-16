using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcBlog.Data.Extensions
{
    public static class PagingDataExtensions
    {
        public static PagingData<T> AsPagingData<T>(this IEnumerable<T> data, QueryRequest query, QueryStatistic statistic)
        {
            Pagination pagination = new Pagination
            {
                TotalCount = statistic.Count
            };

            if (query.Pagination is not null)
            {
                pagination.CurrentPage = query.Pagination.CurrentPage;
                pagination.PageSize = query.Pagination.PageSize;
            }
            else
            {
                pagination.CurrentPage = 0;
                pagination.PageSize = pagination.TotalCount;
            }

            return new PagingData<T>(data, pagination);
        }

        public static async Task<PagingData<T>> AsPagingData<T>(this IAsyncEnumerable<T> data, QueryRequest query, QueryStatistic statistic)
        {
            return (await data.ToArrayAsync().ConfigureAwait(false)).AsPagingData(query, statistic);
        }
    }
}

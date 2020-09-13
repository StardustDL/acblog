using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Linq;

namespace AcBlog.Data.Repositories.Searchers.Local
{
    internal static class LocalSearcherExtensions
    {
        public static QueryResponse<TId> AsQueryResponse<T, TId>(this IEnumerable<T> data, QueryRequest query) where T : IHasId<TId>
        {
            Pagination pagination = new Pagination
            {
                TotalCount = data.Count(),
            };

            if (query.Pagination is not null)
            {
                data = data.Skip(query.Pagination.Offset).Take(query.Pagination.PageSize);
                pagination.CurrentPage = query.Pagination.CurrentPage;
                pagination.PageSize = query.Pagination.PageSize;
            }
            else
            {
                pagination.CurrentPage = 0;
                pagination.PageSize = pagination.TotalCount;
            }

            return new QueryResponse<TId>(data.Select(x => x.Id).ToArray(), pagination);
        }
    }
}

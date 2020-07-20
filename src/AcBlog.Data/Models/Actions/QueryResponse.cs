using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcBlog.Data.Models.Actions
{
    public static class QueryResponse
    {
        public static QueryResponse<T> Empty<T>()
        {
            return new QueryResponse<T>(Array.Empty<T>(), new Pagination());
        }
    }

    public class QueryResponse<T>
    {
        public QueryResponse(IEnumerable<T> results, Pagination currentPage)
        {
            Results = results;
            CurrentPage = currentPage;
        }

        public IEnumerable<T> Results { get; set; }

        public Pagination CurrentPage { get; set; }

        public QueryResponse<T2> Map<T2>(Func<T, T2> mapper)
        {
            return new QueryResponse<T2>(Results.Select(mapper), CurrentPage);
        }

        public async Task<QueryResponse<T2>> MapAsync<T2>(Func<T, Task<T2>> mapper)
        {
            var list = new List<T2>();
            foreach (var v in Results)
                list.Add(await mapper(v));
            return new QueryResponse<T2>(list, CurrentPage);
        }
    }
}

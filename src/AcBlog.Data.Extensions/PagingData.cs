using AcBlog.Data.Models.Actions;
using System.Collections.Generic;

namespace AcBlog.Data
{
    public class PagingData<T>
    {
        public PagingData(IList<T> results)
        {
            Results = results;
            CurrentPage = new Pagination
            {
                CurrentPage = 0,
                PageSize = results.Count,
                TotalCount = results.Count
            };
        }

        public PagingData(IEnumerable<T> results, Pagination currentPage)
        {
            Results = results;
            CurrentPage = currentPage;
        }

        public IEnumerable<T> Results { get; set; }

        public Pagination CurrentPage { get; set; }
    }
}

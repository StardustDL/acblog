using System;

namespace AcBlog.Data.Models.Actions
{
    public class Pagination
    {
        public int CountPerPage { get; set; } = 10;

        public int PageNumber { get; set; } = 0;

        public int TotalCount { get; set; } = 0;

        public int Offset => CountPerPage * PageNumber;

        public int TotalPage => (int)Math.Ceiling((double)TotalCount / CountPerPage);

        public bool HasPreviousPage => Offset > 0;

        public bool HasNextPage => Offset + CountPerPage < TotalCount;

        public Pagination PreviousPage()
        {
            if (!HasPreviousPage)
                throw new System.Exception("No previous page");
            return new Pagination
            {
                CountPerPage = CountPerPage,
                PageNumber = PageNumber - 1,
                TotalCount = TotalCount
            };
        }

        public Pagination NextPage()
        {
            if (!HasNextPage)
                throw new System.Exception("No next page");
            return new Pagination
            {
                CountPerPage = CountPerPage,
                PageNumber = PageNumber + 1,
                TotalCount = TotalCount
            };
        }
    }
}

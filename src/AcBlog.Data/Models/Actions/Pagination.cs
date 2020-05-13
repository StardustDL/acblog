using System;

namespace AcBlog.Data.Models.Actions
{
    public class Pagination
    {
        private int _countPerPage = 10;

        public int CountPerPage
        {
            get => _countPerPage; set
            {
                if (value <= 0) value = 10;
                _countPerPage = value;
            }
        }

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

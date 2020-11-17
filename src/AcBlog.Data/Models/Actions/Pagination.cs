using System;

namespace AcBlog.Data.Models.Actions
{
    public record Pagination
    {
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize; 
            init
            {
                if (value <= 0) value = 10;
                _pageSize = value;
            }
        }

        public int CurrentPage { get; init; }

        public int TotalCount { get; init; }

        public int Offset => PageSize * CurrentPage;

        public int TotalPage => (int)Math.Ceiling((double)TotalCount / PageSize);

        public bool HasPreviousPage => Offset > 0;

        public bool HasNextPage => Offset + PageSize < TotalCount;

        public Pagination PreviousPage()
        {
            if (!HasPreviousPage)
                throw new System.Exception("No previous page");
            return new Pagination
            {
                PageSize = PageSize,
                CurrentPage = CurrentPage - 1,
                TotalCount = TotalCount
            };
        }

        public Pagination NextPage()
        {
            if (!HasNextPage)
                throw new System.Exception("No next page");
            return new Pagination
            {
                PageSize = PageSize,
                CurrentPage = CurrentPage + 1,
                TotalCount = TotalCount
            };
        }
    }
}

using System;

namespace AcBlog.Data.Repositories.FileSystem
{
    public class PagingConfig
    {
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize; set
            {
                if (value <= 0)
                    value = 10;
                _pageSize = value;
            }
        }

        public int TotalCount { get; set; } = 0;

        public int TotalPage => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}

using System;

namespace AcBlog.Data.Repositories.FileSystem
{
    public class UserRepositoryConfig
    {
        public int CountPerPage { get; set; } = 10;

        public int TotalCount { get; set; } = 0;

        public int TotalPage => (int)Math.Ceiling((double)TotalCount / CountPerPage);
    }
}

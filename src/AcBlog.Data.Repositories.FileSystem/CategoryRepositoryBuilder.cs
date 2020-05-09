using AcBlog.Data.Models;
using System.Collections.Generic;
using System.IO;

namespace AcBlog.Data.Repositories.FileSystem
{
    public class CategoryRepositoryBuilder : BaseRepositoryBuilder<Category>
    {
        public CategoryRepositoryBuilder(IList<Category> data, DirectoryInfo dist) : base(data, dist)
        {
        }

        protected override string GetId(Category item) => item.Id;
    }
}

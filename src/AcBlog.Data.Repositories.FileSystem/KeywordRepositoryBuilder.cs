using AcBlog.Data.Models;
using System.Collections.Generic;
using System.IO;

namespace AcBlog.Data.Repositories.FileSystem
{
    public class KeywordRepositoryBuilder : BaseRepositoryBuilder<Keyword>
    {
        public KeywordRepositoryBuilder(IList<Keyword> data, DirectoryInfo dist) : base(data, dist)
        {
        }

        protected override string GetId(Keyword item) => item.Id;
    }
}

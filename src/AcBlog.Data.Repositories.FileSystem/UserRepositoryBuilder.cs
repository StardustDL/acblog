using AcBlog.Data.Models;
using AcBlog.Data.Repositories.FileSystem.Readers;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem
{
    public class UserRepositoryBuilder : BaseRepositoryBuilder<User>
    {
        public UserRepositoryBuilder(IList<User> data, DirectoryInfo dist) : base(data, dist)
        {
        }

        protected override string GetId(User item) => item.Id;
    }
}

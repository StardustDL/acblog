using AcBlog.Data.Models;
using AcBlog.Data.Repositories.FileSystem.Readers;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem
{
    public static class UserRepositoryBuilder
    {
        public static Task Build(IList<User> data, string rootPath, int countPerPage)
        {
            return Task.CompletedTask;
        }
    }
}

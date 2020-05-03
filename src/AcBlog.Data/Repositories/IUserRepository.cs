using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories
{
    public interface IUserRepository : IRecordRepository<User, string>
    {
        Task<QueryResponse<string>> Query(UserQueryRequest query);
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.SQLServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.SQLServer
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(DataContext data)
        {
            Data = data;
        }

        DataContext Data { get; set; }

        public RepositoryAccessContext Context { get; set; }

        public async Task<IEnumerable<string>> All() => await Data.Users.Select(x => x.Id).ToArrayAsync();

        public Task<bool> CanRead() => Task.FromResult(true);

        public Task<bool> CanWrite() => Task.FromResult(true);

        public async Task<string> Create(User value)
        {
            if (string.IsNullOrWhiteSpace(value.Id))
                value.Id = Guid.NewGuid().ToString();
            Data.Users.Add(value);
            await Data.SaveChangesAsync();
            return value.Id;
        }

        public async Task<bool> Delete(string id)
        {
            var item = await Data.Users.FindAsync(id);
            if (item == null)
                return false;
            Data.Users.Remove(item);
            await Data.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exists(string id)
        {
            var item = await Data.Users.FindAsync(id);
            return item != null;
        }

        public async Task<User> Get(string id)
        {
            var item = await Data.Users.FindAsync(id);
            return item;
        }

        public async Task<QueryResponse<string>> Query(UserQueryRequest query)
        {
            var qr = Data.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Nickname))
                qr = qr.Where(x => x.Nickname.Contains(query.Nickname));

            Pagination pagination = new Pagination
            {
                TotalCount = await qr.CountAsync(),
            };

            if (query.Pagination != null)
            {
                qr = qr.Skip(query.Pagination.Offset).Take(query.Pagination.CountPerPage);
                pagination.PageNumber = query.Pagination.PageNumber;
                pagination.CountPerPage = query.Pagination.CountPerPage;
            }
            else
            {
                pagination.PageNumber = 0;
                pagination.CountPerPage = pagination.TotalCount;
            }

            return new QueryResponse<string>(await qr.Select(x => x.Id).ToArrayAsync(), pagination);
        }

        public async Task<bool> Update(User value)
        {
            Data.Users.Update(value);
            await Data.SaveChangesAsync();
            return true;
        }
    }
}

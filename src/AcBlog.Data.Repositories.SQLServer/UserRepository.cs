using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.SQLServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public async Task<IEnumerable<string>> All(CancellationToken cancellationToken = default) => await Data.Users.Select(x => x.Id).ToArrayAsync(cancellationToken);

        public Task<bool> CanRead(CancellationToken cancellationToken = default) => Task.FromResult(true);

        public Task<bool> CanWrite(CancellationToken cancellationToken = default) => Task.FromResult(true);

        public async Task<string> Create(User value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value.Id))
                value.Id = Guid.NewGuid().ToString();
            Data.Users.Add(value);
            await Data.SaveChangesAsync(cancellationToken);
            return value.Id;
        }

        public async Task<bool> Delete(string id, CancellationToken cancellationToken = default)
        {
            var item = await Data.Users.FindAsync(new object[] { id }, cancellationToken);
            if (item is null)
                return false;
            Data.Users.Remove(item);
            await Data.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            var item = await Data.Users.FindAsync(new object[] { id }, cancellationToken);
            return item != null;
        }

        public async Task<User> Get(string id, CancellationToken cancellationToken = default)
        {
            var item = await Data.Users.FindAsync(new object[] { id }, cancellationToken);
            return item;
        }

        public async Task<QueryResponse<string>> Query(UserQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = Data.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Nickname))
                qr = qr.Where(x => x.Nickname.Contains(query.Nickname));

            Pagination pagination = new Pagination
            {
                TotalCount = await qr.CountAsync(cancellationToken),
            };

            if (query.Pagination != null)
            {
                qr = qr.Skip(query.Pagination.Offset).Take(query.Pagination.PageSize);
                pagination.CurrentPage = query.Pagination.CurrentPage;
                pagination.PageSize = query.Pagination.PageSize;
            }
            else
            {
                pagination.CurrentPage = 0;
                pagination.PageSize = pagination.TotalCount;
            }

            return new QueryResponse<string>(await qr.Select(x => x.Id).ToArrayAsync(cancellationToken), pagination);
        }

        public async Task<bool> Update(User value, CancellationToken cancellationToken = default)
        {
            var to = value;

            var item = await Data.Users.FindAsync(new object[] { to.Id }, cancellationToken);
            if (item is null)
                return false;

            item.Nickname = to.Nickname;

            Data.Users.Update(item);
            await Data.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

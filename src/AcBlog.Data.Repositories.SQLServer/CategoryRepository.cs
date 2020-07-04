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
    public class CategoryRepository : ICategoryRepository
    {
        public CategoryRepository(DataContext data)
        {
            Data = data;
        }

        DataContext Data { get; set; }

        public RepositoryAccessContext Context { get; set; }

        public async Task<IEnumerable<string>> All(CancellationToken cancellationToken = default) => await Data.Categories.Select(x => x.Id).ToArrayAsync(cancellationToken);

        public Task<bool> CanRead(CancellationToken cancellationToken = default) => Task.FromResult(true);

        public Task<bool> CanWrite(CancellationToken cancellationToken = default) => Task.FromResult(true);

        public async Task<string> Create(Category value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value.Id))
                value.Id = Guid.NewGuid().ToString();
            Data.Categories.Add(value);
            await Data.SaveChangesAsync(cancellationToken);
            return value.Id;
        }

        public async Task<bool> Delete(string id, CancellationToken cancellationToken = default)
        {
            var item = await Data.Categories.FindAsync(new object[] { id }, cancellationToken);
            if (item is null)
                return false;
            Data.Categories.Remove(item);
            await Data.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            var item = await Data.Categories.FindAsync(new object[] { id }, cancellationToken);
            return item != null;
        }

        public async Task<Category> Get(string id, CancellationToken cancellationToken = default)
        {
            var item = await Data.Categories.FindAsync(new object[] { id }, cancellationToken);
            return item;
        }

        public async Task<QueryResponse<string>> Query(CategoryQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = Data.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
                qr = qr.Where(x => x.Name.Contains(query.Name));

            Pagination pagination = new Pagination
            {
                TotalCount = await qr.CountAsync(cancellationToken),
            };

            if (query.Pagination != null)
            {
                qr = qr.Skip(query.Pagination.Offset).Take(query.Pagination.CountPerPage);
                pagination.CurrentPage = query.Pagination.PageNumber;
                pagination.PageSize = query.Pagination.CountPerPage;
            }
            else
            {
                pagination.CurrentPage = 0;
                pagination.PageSize = pagination.TotalCount;
            }

            return new QueryResponse<string>(await qr.Select(x => x.Id).ToArrayAsync(cancellationToken), pagination);
        }

        public async Task<bool> Update(Category value, CancellationToken cancellationToken = default)
        {
            var to = value;

            var item = await Data.Categories.FindAsync(new object[] { to.Id }, cancellationToken);
            if (item is null)
                return false;

            item.Name = to.Name;
            item.ParentId = to.ParentId;

            Data.Categories.Update(item);

            await Data.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.SQLServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.SQLServer
{
    public class PostRepository : IPostRepository
    {
        public PostRepository(DataContext data)
        {
            Data = data;
        }

        DataContext Data { get; set; }

        public RepositoryAccessContext Context { get; set; }

        public async Task<IEnumerable<string>> All() => await Data.Posts.Select(x => x.Id).ToArrayAsync();

        public Task<bool> CanRead() => Task.FromResult(true);

        public Task<bool> CanWrite() => Task.FromResult(true);

        public async Task<string> Create(Post value)
        {
            if (string.IsNullOrWhiteSpace(value.Id))
                value.Id = Guid.NewGuid().ToString();
            Data.Posts.Add(PostData.From(value));
            await Data.SaveChangesAsync();
            return value.Id;
        }

        public async Task<bool> Delete(string id)
        {
            var item = await Data.Posts.FindAsync(id);
            if (item == null)
                return false;
            Data.Posts.Remove(item);
            await Data.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exists(string id)
        {
            var item = await Data.Posts.FindAsync(id);
            return item != null;
        }

        public async Task<Post> Get(string id)
        {
            var item = await Data.Posts.FindAsync(id);
            return PostData.To(item);
        }

        public async Task<QueryResponse<string>> Query(PostQueryRequest query)
        {
            var qr = Data.Posts.AsQueryable();

            if (query.Type != null)
                qr = qr.Where(x => x.Type == query.Type);
            if (!string.IsNullOrWhiteSpace(query.AuthorId))
                qr = qr.Where(x => x.AuthorId == query.AuthorId);
            if (!string.IsNullOrWhiteSpace(query.CategoryId))
                qr = qr.Where(x => x.CategoryId == query.CategoryId);
            if (!string.IsNullOrWhiteSpace(query.KeywordId))
                qr = qr.Where(x => x.KeywordIds.Contains(query.KeywordId));
            if (!string.IsNullOrWhiteSpace(query.CategoryId))
                qr = qr.Where(x => x.CategoryId == query.CategoryId);
            if (!string.IsNullOrWhiteSpace(query.Title))
                qr = qr.Where(x => x.Title.Contains(query.Title));
            if (!string.IsNullOrWhiteSpace(query.Content))
                qr = qr.Where(x => x.Content.Contains(query.Content));
            qr = query.Order switch
            {
                PostResponseOrder.None => qr,
                PostResponseOrder.CreationTimeAscending => qr.OrderBy(x => x.CreationTime),
                PostResponseOrder.CreationTimeDescending => qr.OrderByDescending(x => x.CreationTime),
                PostResponseOrder.ModificationTimeAscending => qr.OrderBy(x => x.ModificationTime),
                PostResponseOrder.ModificationTimeDescending => qr.OrderByDescending(x => x.ModificationTime),
                _ => throw new NotImplementedException(),
            };

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

        public async Task<bool> Update(Post value)
        {
            Data.Posts.Update(PostData.From(value));
            await Data.SaveChangesAsync();
            return true;
        }
    }
}

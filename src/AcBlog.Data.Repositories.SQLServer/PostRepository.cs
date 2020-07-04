using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.SQLServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public async Task<IEnumerable<string>> All(CancellationToken cancellationToken = default) => await Data.Posts.Select(x => x.Id).ToArrayAsync(cancellationToken);

        public Task<bool> CanRead(CancellationToken cancellationToken = default) => Task.FromResult(true);

        public Task<bool> CanWrite(CancellationToken cancellationToken = default) => Task.FromResult(true);

        public async Task<string> Create(Post value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value.Id))
                value.Id = Guid.NewGuid().ToString();
            Data.Posts.Add(PostData.From(value));
            await Data.SaveChangesAsync(cancellationToken);
            return value.Id;
        }

        public async Task<bool> Delete(string id,CancellationToken cancellationToken = default)
        {
            var item = await Data.Posts.FindAsync(new object[] { id },cancellationToken);
            if (item is null)
                return false;
            Data.Posts.Remove(item);
            await Data.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            var item = await Data.Posts.FindAsync(new object[] { id }, cancellationToken);
            return item != null;
        }

        public async Task<Post> Get(string id, CancellationToken cancellationToken = default)
        {
            var item = await Data.Posts.FindAsync(new object[] { id }, cancellationToken);
            return PostData.To(item);
        }

        public async Task<QueryResponse<string>> Query(PostQueryRequest query, CancellationToken cancellationToken = default)
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

        public async Task<bool> Update(Post value, CancellationToken cancellationToken = default)
        {
            var to = PostData.From(value);

            var item = await Data.Posts.FindAsync(new object[] { to.Id }, cancellationToken);
            if (item is null)
                return false;

            item.KeywordIds = to.KeywordIds;
            item.CategoryId = to.CategoryId;
            item.AuthorId = to.AuthorId;
            item.Content = to.Content;
            item.CreationTime = to.CreationTime;
            item.ModificationTime = to.ModificationTime;
            item.Title = to.Title;
            item.Type = to.Type;

            Data.Posts.Update(item);
            await Data.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

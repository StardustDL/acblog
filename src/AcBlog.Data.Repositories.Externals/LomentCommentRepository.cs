using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using Loment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Externals
{
    public class LomentCommentRepository : ICommentRepository
    {
        public LomentCommentRepository(ILomentService service)
        {
            Service = service;
        }

        readonly Lazy<RepositoryStatus> _status = new Lazy<RepositoryStatus>(new RepositoryStatus
        {
            CanRead = true,
            CanWrite = true,
        });

        public RepositoryAccessContext Context { get; set; } = new RepositoryAccessContext();

        public ILomentService Service { get; }

        public async IAsyncEnumerable<string> All([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var items = await Service.Query(new Loment.Models.CommentQuery
            {
                Offset = 0,
                Limit = int.MaxValue,
            }, cancellationToken).ConfigureAwait(false);

            foreach (var item in items.Select(x => x.Id))
                yield return item;
        }

        public Task<string?> Create(Comment value, CancellationToken cancellationToken = default)
        {
            return Service.Create(new Loment.Models.Comment
            {
                Author = value.Author,
                Content = value.Content,
                CreationTime = value.CreationTime,
                Email = value.Email,
                Extra = value.Extra,
                Id = value.Id,
                Link = value.Link,
                ModificationTime = value.ModificationTime,
                Uri = value.Uri
            }, cancellationToken);
        }

        public Task<bool> Delete(string id, CancellationToken cancellationToken = default)
        {
            return Service.Delete(id, cancellationToken);
        }

        public async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await Get(id, cancellationToken).ConfigureAwait(false);
                return result is not null;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Comment?> Get(string id, CancellationToken cancellationToken = default)
        {
            var value = await Service.Get(id, cancellationToken).ConfigureAwait(false);
            if (value is null)
                return null;
            return new Comment
            {
                Author = value.Author,
                Content = value.Content,
                CreationTime = value.CreationTime,
                Email = value.Email,
                Extra = value.Extra,
                Id = value.Id,
                Link = value.Link,
                ModificationTime = value.ModificationTime,
                Uri = value.Uri
            };
        }

        public Task<RepositoryStatus> GetStatus(CancellationToken cancellationToken = default) => Task.FromResult(_status.Value);

        static Loment.Models.CommentQuery ToInnerQuery(CommentQueryRequest query)
        {
            var pagination = query.Pagination ?? new Pagination
            {
                PageSize = int.MaxValue,
                CurrentPage = 0
            };

            var innerQuery = new Loment.Models.CommentQuery
            {
                Author = query.Author,
                Content = query.Content,
                Email = query.Email,
                Link = query.Link,
                Uri = query.Uri,
                Offset = pagination.Offset,
                Limit = pagination.PageSize,
            };
            return innerQuery;
        }

        public async IAsyncEnumerable<string> Query(CommentQueryRequest query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var innerQuery = ToInnerQuery(query);

            var items = await Service.Query(innerQuery, cancellationToken).ConfigureAwait(false);

            foreach (var item in items.Select(x => x.Id))
                yield return item;
        }

        public async Task<QueryStatistic> Statistic(CommentQueryRequest query, CancellationToken cancellationToken = default)
        {
            var innerQuery = ToInnerQuery(query);
            var count = await Service.Count(innerQuery, cancellationToken).ConfigureAwait(false);
            return new QueryStatistic
            {
                Count = (int)count
            };
        }

        public Task<bool> Update(Comment value, CancellationToken cancellationToken = default)
        {
            return Service.Update(new Loment.Models.Comment
            {
                Author = value.Author,
                Content = value.Content,
                CreationTime = value.CreationTime,
                Email = value.Email,
                Extra = value.Extra,
                Id = value.Id,
                Link = value.Link,
                ModificationTime = value.ModificationTime,
                Uri = value.Uri
            }, cancellationToken);
        }
    }
}

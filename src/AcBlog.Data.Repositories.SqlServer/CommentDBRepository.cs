using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.SqlServer
{
    public class CommentDBRepository : RecordDBRepository<Comment, string, CommentQueryRequest, RawComment>, ICommentRepository
    {
        public CommentDBRepository(BlogDataContext dataSource) : base(dataSource)
        {
        }

        protected override DbSet<RawComment> DbSet => DataSource.Comments;

        protected override IQueryable<string> InnerQuery(CommentQueryRequest query)
        {
            var qr = DbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Uri))
            {
                qr = qr.Where(x => x.Uri == query.Uri);
            }
            if (!string.IsNullOrWhiteSpace(query.Author))
            {
                qr = qr.Where(x => x.Author == query.Author);
            }
            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                qr = qr.Where(x => x.Email == query.Email);
            }
            if (!string.IsNullOrWhiteSpace(query.Link))
            {
                qr = qr.Where(x => x.Link == query.Link);
            }
            if (!string.IsNullOrWhiteSpace(query.Content))
            {
                qr = qr.Where(x => x.Content.Contains(query.Content));
            }

            qr = query.Order switch
            {
                QueryTimeOrder.None => qr.OrderBy(x => x.CreationTime),
                QueryTimeOrder.CreationTimeAscending => qr.OrderBy(x => x.CreationTime),
                QueryTimeOrder.CreationTimeDescending => qr.OrderByDescending(x => x.CreationTime),
                QueryTimeOrder.ModificationTimeAscending => qr.OrderBy(x => x.ModificationTime),
                QueryTimeOrder.ModificationTimeDescending => qr.OrderByDescending(x => x.ModificationTime),
                _ => throw new NotImplementedException(),
            };

            if (query.Pagination is not null)
            {
                qr = qr.Skip(query.Pagination.Offset).Take(query.Pagination.PageSize);
            }

            return qr.Select(x => x.Id);
        }

        public override Task<string> Create(Comment value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value.Id))
                value.Id = Guid.NewGuid().ToString();
            return base.Create(value, cancellationToken);
        }

        protected override RawComment ToRaw(Comment item) => RawComment.From(item);

        protected override Comment ToModel(RawComment item) => RawComment.To(item);

        protected override void ApplyChanges(RawComment target, RawComment value)
        {
            target.Content = value.Content;
            target.Author = value.Author;
            target.Email = value.Email;
            target.Extra = value.Extra;
            target.Link = value.Link;
            target.Uri = value.Uri;
            target.CreationTime = value.CreationTime;
            target.ModificationTime = value.ModificationTime;
        }
    }
}

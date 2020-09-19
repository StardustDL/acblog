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
    public class UserDBRepository : RecordDBRepository<User, string, UserQueryRequest, RawUser>, IUserRepository
    {
        public UserDBRepository(BlogDataContext dataSource) : base(dataSource)
        {
        }

        protected override DbSet<RawUser> DbSet => DataSource.Users;

        public override Task<string> Create(User value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value.Id))
                value.Id = Guid.NewGuid().ToString();
            return base.Create(value, cancellationToken);
        }

        protected override IQueryable<string> InnerQuery(UserQueryRequest query)
        {
            var qr = DbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.NickName))
            {
                qr = qr.Where(x => x.Name == query.NickName);
            }

            qr = qr.OrderBy(x => x.Id);

            if (query.Pagination is not null)
            {
                qr = qr.Skip(query.Pagination.Offset).Take(query.Pagination.PageSize);
            }

            return qr.Select(x => x.Id);
        }

        protected override RawUser ToRaw(User item) => RawUser.From(item);

        protected override User ToModel(RawUser item) => RawUser.To(item);

        protected override void ApplyChanges(RawUser target, RawUser value)
        {
            target.Name = value.Name;
            target.Email = value.Email;
        }
    }
}

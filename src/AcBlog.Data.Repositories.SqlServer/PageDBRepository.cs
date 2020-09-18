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
    public class PageDBRepository : RecordDBRepository<Page, string, PageQueryRequest, RawPage>, IPageRepository
    {
        public PageDBRepository(BlogDataContext dataSource) : base(dataSource)
        {
        }

        protected override DbSet<RawPage> DbSet => DataSource.Pages;

        public override Task<string> Create(Page value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value.Id))
                value.Id = Guid.NewGuid().ToString();
            return base.Create(value, cancellationToken);
        }

        protected override IQueryable<string> InnerQuery(PageQueryRequest query)
        {
            var qr = DbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Route))
            {
                qr = qr.Where(x => x.Route == query.Route);
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

        protected override RawPage ToRaw(Page item) => RawPage.From(item);

        protected override Page ToModel(RawPage item) => RawPage.To(item);

        protected override void ApplyChanges(RawPage target, RawPage value)
        {
            target.Features = value.Features;
            target.Layout = value.Layout;
            target.Properties = value.Properties;
            target.Route = value.Route;
            target.Title = value.Title;
            target.Content = value.Content;
            target.CreationTime = value.CreationTime;
            target.ModificationTime = value.ModificationTime;
        }
    }
}

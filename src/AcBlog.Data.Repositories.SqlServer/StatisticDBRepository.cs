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
    public class StatisticDBRepository : RecordDBRepository<Statistic, string, StatisticQueryRequest, RawStatistic>, IStatisticRepository
    {
        public StatisticDBRepository(BlogDataContext dataSource) : base(dataSource)
        {
        }

        protected override DbSet<RawStatistic> DbSet => DataSource.Statistics;

        protected override IQueryable<string> InnerQuery(StatisticQueryRequest query)
        {
            var qr = DbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Uri))
            {
                qr = qr.Where(x => x.Uri == query.Uri);
            }
            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                qr = qr.Where(x => x.Category == query.Category);
            }
            if (!string.IsNullOrWhiteSpace(query.Payload))
            {
                qr = qr.Where(x => x.Payload.Contains(query.Payload));
            }

            qr = query.Order switch
            {
                QueryTimeOrder.None => qr,
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

        public override Task<string> Create(Statistic value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value.Id))
                value.Id = Guid.NewGuid().ToString();
            return base.Create(value, cancellationToken);
        }

        protected override RawStatistic ToRaw(Statistic item) => RawStatistic.From(item);

        protected override Statistic ToModel(RawStatistic item) => RawStatistic.To(item);

        protected override void ApplyChanges(RawStatistic target, RawStatistic value)
        {
            target.Category = value.Category;
            target.Payload = value.Payload;
            target.Uri = value.Uri;
            target.CreationTime = value.CreationTime;
            target.ModificationTime = value.ModificationTime;
        }
    }
}

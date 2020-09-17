using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.SqlServer
{
    public class LayoutDBRepository : RecordDBRepository<Layout, string, LayoutQueryRequest, RawLayout>, ILayoutRepository
    {
        public LayoutDBRepository(BlogDataContext dataSource) : base(dataSource)
        {
        }

        protected override DbSet<RawLayout> DbSet => DataSource.Layouts;

        public override Task<string> Create(Layout value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value.Id))
                value.Id = Guid.NewGuid().ToString();
            return base.Create(value, cancellationToken);
        }

        protected override RawLayout ToRaw(Layout item) => RawLayout.From(item);

        protected override Layout ToModel(RawLayout item) => RawLayout.To(item);

        protected override void ApplyChanges(RawLayout target, RawLayout value)
        {
            target.Template = value.Template;
            target.CreationTime = value.CreationTime;
            target.ModificationTime = value.ModificationTime;
        }
    }
}

using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.SqlServer
{
    public class PostDBRepository : RecordDBRepository<Post, string, PostQueryRequest, RawPost>, IPostRepository
    {
        public PostDBRepository(BlogDataContext dataSource) : base(dataSource)
        {
        }

        protected override DbSet<RawPost> DbSet => DataSource.Posts;

        public override Task<string> Create(Post value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value.Id))
                value.Id = Guid.NewGuid().ToString();
            return base.Create(value, cancellationToken);
        }

        protected override IQueryable<string> InnerQuery(PostQueryRequest query)
        {
            var qr = DbSet.AsQueryable();

            if (query.Type is not null)
                qr = qr.Where(x => x.Type == query.Type);
            if (!string.IsNullOrWhiteSpace(query.Author))
                qr = qr.Where(x => x.Author == query.Author);
            if (query.Category is not null)
                qr = qr.Where(x => x.Category.StartsWith(query.Category.ToString()));
            if (query.Keywords is not null)
                qr = qr.Where(x => x.Keywords.StartsWith(query.Keywords.ToString()));
            if (!string.IsNullOrWhiteSpace(query.Title))
                qr = qr.Where(x => x.Title.Contains(query.Title));
            if (!string.IsNullOrWhiteSpace(query.Content))
            {
                string jsonContent = JsonSerializer.Serialize(query.Content);
                qr = qr.Where(x => x.Content.Contains(jsonContent));
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

        public async Task<CategoryTree> GetCategories(CancellationToken cancellationToken = default)
        {
            var cates = DbSet.AsQueryable().Select(x => x.Category).Distinct().AsAsyncEnumerable();
            return await CategoryTreeBuilder.Build(cates.Select(x => Category.Parse(x)), cancellationToken);
        }

        public async Task<KeywordCollection> GetKeywords(CancellationToken cancellationToken = default)
        {
            var cates = DbSet.AsQueryable().Select(x => x.Keywords).Distinct().AsAsyncEnumerable();
            return await KeywordCollectionBuilder.Build(cates.Select(x => Keyword.Parse(x)), cancellationToken);
        }

        protected override RawPost ToRaw(Post item) => RawPost.From(item);

        protected override Post ToModel(RawPost item) => RawPost.To(item);

        protected override void ApplyChanges(RawPost target, RawPost value)
        {
            target.Keywords = value.Keywords;
            target.Category = value.Category;
            target.Author = value.Author;
            target.Content = value.Content;
            target.CreationTime = value.CreationTime;
            target.ModificationTime = value.ModificationTime;
            target.Title = value.Title;
            target.Type = value.Type;
        }
    }
}

using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using Scriban.Syntax;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers.Local
{
    public class LocalPostRepositorySearcher : IPostRepositorySearcher
    {
        public LocalPostRepositorySearcher(IPostRepository repository) => Repository = repository;

        public IPostRepository Repository { get; }

        public async Task<QueryResponse<string>> Search(PostQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = (await Repository.GetAllItems(cancellationToken)).IgnoreNull();

            if (query.Type != null)
                qr = qr.Where(x => x.Type == query.Type);
            if (!string.IsNullOrWhiteSpace(query.Author))
                qr = qr.Where(x => x.Author == query.Author);
            if (query.Category != null)
                qr = qr.Where(x => x.Category.ToString().StartsWith(query.Category.ToString()));
            if (query.Keywords != null)
                qr = qr.Where(x => query.Keywords.Items.All(k => x.Keywords.Items.Contains(k)));
            if (!string.IsNullOrWhiteSpace(query.Title))
                qr = qr.Where(x => x.Title.Contains(query.Title));
            if (!string.IsNullOrWhiteSpace(query.Content))
            {
                string jsonContent = JsonSerializer.Serialize(query.Content);
                qr = qr.Where(x => x.Content.Raw.Contains(jsonContent));
            }
            qr = query.Order switch
            {
                PostResponseOrder.None => qr,
                PostResponseOrder.CreationTimeAscending => qr.OrderBy(x => x.CreationTime),
                PostResponseOrder.CreationTimeDescending => qr.OrderByDescending(x => x.CreationTime),
                PostResponseOrder.ModificationTimeAscending => qr.OrderBy(x => x.ModificationTime),
                PostResponseOrder.ModificationTimeDescending => qr.OrderByDescending(x => x.ModificationTime),
                _ => throw new NotImplementedException(),
            };

            return qr.AsQueryResponse<Post, string>(query);
        }
    }
}

using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers.Local
{
    public class LocalPostRepositorySearcher : IPostRepositorySearcher
    {
        public IAsyncEnumerable<string> Search(IPostRepository repository, PostQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = repository.GetAllItems(cancellationToken).IgnoreNull();

            if (query.Type is not null)
                qr = qr.Where(x => x.Type == query.Type);
            if (!string.IsNullOrWhiteSpace(query.Author))
                qr = qr.Where(x => x.Author == query.Author);
            if (query.Category is not null)
                qr = qr.Where(x => x.Category.ToString().StartsWith(query.Category.ToString()));
            if (query.Keywords is not null)
                qr = qr.Where(x => query.Keywords.Items.All(k => x.Keywords.Items.Contains(k)));
            if (!string.IsNullOrWhiteSpace(query.Title))
                qr = qr.Where(x => x.Title.Contains(query.Title));
            if (!string.IsNullOrWhiteSpace(query.Content))
            {
                qr = qr.Where(x => x.Content.Raw.Contains(query.Content));
            }
            if (!string.IsNullOrWhiteSpace(query.Term))
            {
                qr = qr.Where(x =>
                    x.Author.Contains(query.Term) ||
                    x.Category.ToString().Contains(query.Term) ||
                    x.Keywords.ToString().Contains(query.Term) ||
                    x.Title.ToString().Contains(query.Term) ||
                    x.Content.Raw.ToString().Contains(query.Term)
                );
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

            return qr.Select(item => item.Id).IgnoreNull().Paging(query.Pagination);
        }
    }
}

using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers.Local
{
    public class LocalCommentRepositorySearcher : ICommentRepositorySearcher
    {
        public IAsyncEnumerable<string> Search(ICommentRepository repository, CommentQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = repository.GetAllItems(cancellationToken).IgnoreNull();

            if (!string.IsNullOrWhiteSpace(query.Author))
                qr = qr.Where(x => x.Author == query.Author);
            if (!string.IsNullOrWhiteSpace(query.Email))
                qr = qr.Where(x => x.Email == query.Email);
            if (!string.IsNullOrWhiteSpace(query.Link))
                qr = qr.Where(x => x.Link == query.Link);
            if (!string.IsNullOrWhiteSpace(query.Uri))
                qr = qr.Where(x => x.Uri == query.Uri);
            if (!string.IsNullOrWhiteSpace(query.Content))
                qr = qr.Where(x => x.Content.Contains(query.Content));
            if (!string.IsNullOrWhiteSpace(query.Term))
            {
                qr = qr.Where(x =>
                    x.Author.Contains(query.Term) ||
                    x.Content.ToString().Contains(query.Term)
                );
            }

            return qr.Select(item => item.Id).IgnoreNull().Paging(query.Pagination);
        }
    }
}

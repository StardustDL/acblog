using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers.Local
{
    public class LocalCommentRepositorySearcher : ICommentRepositorySearcher
    {
        public LocalCommentRepositorySearcher(ICommentRepository repository) => Repository = repository;

        public ICommentRepository Repository { get; }

        public async Task<QueryResponse<string>> Search(CommentQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = (await Repository.GetAllItems(cancellationToken)).IgnoreNull();

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

            return qr.AsQueryResponse<Comment, string>(query);
        }
    }
}

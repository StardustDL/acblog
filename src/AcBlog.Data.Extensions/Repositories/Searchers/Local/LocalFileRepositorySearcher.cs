using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers.Local
{
    public class LocalFileRepositorySearcher : IFileRepositorySearcher
    {
        public IAsyncEnumerable<string> Search(IFileRepository repository, FileQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = repository.GetAllItems(cancellationToken).IgnoreNull();

            return qr.Select(item => item.Id).IgnoreNull().Paging(query.Pagination);
        }
    }
}

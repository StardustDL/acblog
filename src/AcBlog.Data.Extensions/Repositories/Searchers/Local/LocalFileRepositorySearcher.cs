using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers.Local
{
    public class LocalFileRepositorySearcher : IFileRepositorySearcher
    {
        public LocalFileRepositorySearcher(IFileRepository repository) => Repository = repository;

        public IFileRepository Repository { get; }

        public async Task<QueryResponse<string>> Search(FileQueryRequest query, CancellationToken cancellationToken = default)
        {
            var qr = Repository.GetAllItems(cancellationToken).IgnoreNull();

            return (await qr.ToArrayAsync(cancellationToken)).AsQueryResponse<File, string>(query);
        }
    }
}

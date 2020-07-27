using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
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
            var qr = (await Repository.GetAllItems(cancellationToken)).IgnoreNull();

            return qr.AsQueryResponse<File, string>(query);
        }
    }
}

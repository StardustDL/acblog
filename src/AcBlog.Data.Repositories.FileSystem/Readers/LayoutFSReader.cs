using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.Searchers.Local;
using StardustDL.Extensions.FileProviders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public class LayoutFSReader : RecordFSReaderBase<Layout, string, LayoutQueryRequest>, ILayoutRepository
    {
        public LayoutFSReader(string rootPath, IFileProvider fileProvider) : base(rootPath, fileProvider)
        {
        }

        protected override IAsyncEnumerable<string>? FullQuery(LayoutQueryRequest query, CancellationToken cancellationToken = default)
        {
            return new LocalLayoutRepositorySearcher().Search(this, query, cancellationToken);
        }
    }
}

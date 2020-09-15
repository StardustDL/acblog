using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.Searchers.Local;
using StardustDL.Extensions.FileProviders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public class FileFSReader : RecordFSReaderBase<File, string, FileQueryRequest>, IFileRepository
    {
        public FileFSReader(string rootPath, IFileProvider fileProvider) : base(rootPath, fileProvider)
        {
        }

        protected override IAsyncEnumerable<string>? FullQuery(FileQueryRequest query, CancellationToken cancellationToken = default)
        {
            return new LocalFileRepositorySearcher().Search(this, query, cancellationToken);
        }
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using StardustDL.Extensions.FileProviders;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public class LayoutFSReader : RecordFSReaderBase<Layout, string, LayoutQueryRequest>, ILayoutRepository
    {
        public LayoutFSReader(string rootPath, IFileProvider fileProvider) : base(rootPath, fileProvider)
        {
        }

        protected override string GetPath(string id) => Paths.GetFileById(RootPath, id);

        public override Task<QueryResponse<string>> Query(LayoutQueryRequest query, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(QueryResponse.Empty<string>());
        }
    }
}

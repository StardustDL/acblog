using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using StardustDL.Extensions.FileProviders;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public class PageFSReader : RecordFSReaderBase<Page, string, PageQueryRequest>, IPageRepository
    {
        public PageFSReader(string rootPath, IFileProvider fileProvider) : base(rootPath, fileProvider)
        {
        }

        protected override string GetPath(string id) => Paths.GetFileById(RootPath, id);

        public override async Task<QueryResponse<string>> Query(PageQueryRequest query, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(query.Route))
            {
                string path = Paths.GetRouteFile(RootPath, query.Route);
                using var fs = await (await FileProvider.GetFileInfo(path).ConfigureAwait(false)).CreateReadStream().ConfigureAwait(false);
                var result = await JsonSerializer.DeserializeAsync<string[]>(fs, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                return new QueryResponse<string>(result);
            }
            return await base.Query(query, cancellationToken).ConfigureAwait(false);
        }
    }
}

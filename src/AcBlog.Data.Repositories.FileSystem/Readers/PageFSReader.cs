using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.Searchers.Local;
using StardustDL.Extensions.FileProviders;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        protected override IAsyncEnumerable<string>? EfficientQuery(PageQueryRequest query, CancellationToken cancellationToken = default)
        {
            async IAsyncEnumerable<string> ByRoute()
            {
                string path = Paths.GetRouteFile(RootPath, query.Route);
                await using var fs = await (await FileProvider.GetFileInfo(path).ConfigureAwait(false)).CreateReadStream().ConfigureAwait(false);
                var result = await JsonSerializer.DeserializeAsync<string[]>(fs, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                if (result is not null)
                {
                    foreach (var item in result)
                        yield return item;
                }
            }

            if (!string.IsNullOrEmpty(query.Route))
            {
                return ByRoute().Paging(query.Pagination);
            }
            return null;
        }

        protected override IAsyncEnumerable<string>? FullQuery(PageQueryRequest query, CancellationToken cancellationToken = default)
        {
            return new LocalPageRepositorySearcher().Search(this, query, cancellationToken);
        }
    }
}

using AcBlog.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Builders
{
    public class PageRepositoryBuilder : RecoderRepositoryBuilderBase<Page, string>
    {
        public PageRepositoryBuilder(string rootPath) : base(rootPath)
        {
        }

        async Task BuildRouteIndex(IList<Page> data)
        {
            var gr = from x in data group x by x.Route;

            foreach (var g in gr)
            {
                string path = Paths.GetRouteFile(RootPath, g.Key);
                await using var st = FSStaticBuilder.GetFileRewriteStream(path);
                await JsonSerializer.SerializeAsync(st, (from p in g select p.Id).ToArray()).ConfigureAwait(false);
            }
        }

        public override async Task Build(IList<Page> data)
        {
            await base.Build(data).ConfigureAwait(false);
            await BuildRouteIndex(data).ConfigureAwait(false);
        }
    }
}

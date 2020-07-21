using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AcBlog.Data.Models.Actions;
using StardustDL.Extensions.FileProviders;

namespace AcBlog.Data.Repositories.FileSystem
{

    // config.json, 0.json, 1.json, ...

    public class PagingProvider<TId>
    {
        public PagingProvider(string rootPath, IFileProvider? fileProvider = null)
        {
            RootPath = rootPath;
            FileProvider = fileProvider ?? new Microsoft.Extensions.FileProviders.NullFileProvider().AsFileProvider();
        }

        public async Task EnsureConfig()
        {
            using var fs = await (await FileProvider.GetFileInfo(GetConfigPath()).ConfigureAwait(false)).CreateReadStream().ConfigureAwait(false);
            Config = await JsonSerializer.DeserializeAsync<PagingConfig>(fs).ConfigureAwait(false);
        }

        public string RootPath { get; }

        protected IFileProvider FileProvider { get; }

        public PagingConfig? Config { get; private set; } = null;

        public string GetConfigPath() => Path.Join(RootPath, "config.json");

        public string GetPagePath(int pageNumber) => Path.Join(RootPath, $"{pageNumber}.json");

        public async Task FillPagination(Pagination pagination)
        {
            await EnsureConfig();
            pagination.PageSize = Config!.PageSize;
            pagination.TotalCount = Config.TotalCount;
        }

        public async Task<IList<TId>> GetPaging(Pagination pagination)
        {
            await EnsureConfig();

            if (pagination.CurrentPage >= 0 &&
                (pagination.CurrentPage < Config!.TotalPage ||
                Config.TotalPage == 0 && pagination.CurrentPage == 0))
            {
                string path = GetPagePath(pagination.CurrentPage);
                using var fs = await (await FileProvider.GetFileInfo(path).ConfigureAwait(false)).CreateReadStream().ConfigureAwait(false);
                return await JsonSerializer.DeserializeAsync<IList<TId>>(fs).ConfigureAwait(false);
            }
            else
            {
                return Array.Empty<TId>();
            }
        }

        public async Task Build(IList<TId> data, PagingConfig config, CancellationToken cancellationToken = default)
        {
            FSBuilder builder = new FSBuilder(RootPath);
            builder.EnsureDirectoryEmpty();

            {
                string configPath = Path.GetRelativePath(RootPath, GetConfigPath());

                config.TotalCount = data.Count;

                using var st = builder.GetFileRewriteStream(configPath);

                await JsonSerializer.SerializeAsync(
                    st, config, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }

            List<TId> page = new List<TId>();
            int pn = 0;
            foreach (var v in data)
            {
                page.Add(v);
                if (page.Count == config.PageSize)
                {
                    string pagePath = Path.GetRelativePath(RootPath, GetPagePath(pn));

                    using var st = builder.GetFileRewriteStream(pagePath);

                    await JsonSerializer.SerializeAsync(
                        st, page, cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
                    page.Clear();
                    pn++;
                }
            }
            if (page.Count > 0 || data.Count == 0)
            {
                string pagePath = Path.GetRelativePath(RootPath, GetPagePath(pn));

                using var st = builder.GetFileRewriteStream(pagePath);

                await JsonSerializer.SerializeAsync(
                    st, page, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}

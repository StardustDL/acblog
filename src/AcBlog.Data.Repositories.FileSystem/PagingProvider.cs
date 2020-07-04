using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AcBlog.Data.Models.Actions;
using Microsoft.Extensions.FileProviders;

namespace AcBlog.Data.Repositories.FileSystem
{
    // config.json, 0.json, 1.json, ...

    public class PagingProvider<TId>
    {
        Lazy<PagingConfig> _config;

        public PagingProvider(string rootPath, IFileProvider? fileProvider = null)
        {
            RootPath = rootPath;
            FileProvider = fileProvider ?? new NullFileProvider();
            _config = new Lazy<PagingConfig>(() =>
            {
                using var fs = FileProvider.GetFileInfo(GetConfigPath()).CreateReadStream();
                return JsonSerializer.DeserializeAsync<PagingConfig>(fs).ConfigureAwait(false).GetAwaiter().GetResult();
            });
        }

        public string RootPath { get; }

        protected IFileProvider FileProvider { get; }

        public PagingConfig Config => _config.Value;

        public string GetConfigPath() => Path.Join(RootPath, "config.json");

        public string GetPagePath(int pageNumber) => Path.Join(RootPath, $"{pageNumber}.json");

        public void FillPagination(Pagination pagination)
        {
            pagination.PageSize = Config.PageSize;
            pagination.TotalCount = Config.TotalCount;
        }

        public IList<TId> GetPaging(Pagination pagination)
        {
            if (pagination.CurrentPage >= 0 &&
                (pagination.CurrentPage < Config.TotalPage ||
                Config.TotalPage == 0 && pagination.CurrentPage == 0))
            {
                string path = GetPagePath(pagination.CurrentPage);
                using var fs = FileProvider.GetFileInfo(path).CreateReadStream();
                return JsonSerializer.DeserializeAsync<IList<TId>>(fs)
                    .ConfigureAwait(false).GetAwaiter().GetResult();
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

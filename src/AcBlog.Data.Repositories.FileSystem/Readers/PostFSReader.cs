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
    public class PostFSReader : RecordFSReaderBase<Post, string, PostQueryRequest>, IPostRepository
    {
        public PostFSReader(string rootPath, IFileProvider fileProvider) : base(rootPath, fileProvider)
        {
        }

        protected override async IAsyncEnumerable<string>? EfficientQuery(PostQueryRequest query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            query.Pagination ??= new Pagination();

            var rootPath = Paths.GetConfigRoot(RootPath);

            if (query.Type is not null)
            {
                switch (query.Type)
                {
                    case PostType.Article:
                        rootPath = Paths.GetArticleRoot(RootPath);
                        break;
                    case PostType.Slides:
                        rootPath = Paths.GetSlidesRoot(RootPath);
                        break;
                    case PostType.Note:
                        rootPath = Paths.GetNoteRoot(RootPath);
                        break;
                }
            }
            else if (query.Category is not null && query.Category.Items.Any())
            {
                rootPath = Paths.GetCategoryRoot(RootPath, query.Category);
            }
            else if (query.Keywords is not null && query.Keywords.Items.Any())
            {
                rootPath = Paths.GetKeywordRoot(RootPath, query.Keywords);
            }

            var list = await GetIdList(rootPath, cancellationToken).ConfigureAwait(false);
            foreach (var item in list)
            {
                yield return item;
            }
        }

        protected override IAsyncEnumerable<string>? FullQuery(PostQueryRequest query, CancellationToken cancellationToken = default)
        {
            return new LocalPostRepositorySearcher().Search(this, query, cancellationToken);
        }

        public async Task<CategoryTree> GetCategories(CancellationToken cancellationToken = default)
        {
            await using var fs = await (await FileProvider.GetFileInfo(Paths.GetCategoryMetadata(RootPath)).ConfigureAwait(false)).CreateReadStream().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<CategoryTree>(fs, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return result ?? throw new System.Exception("Categories are null");
        }

        public async Task<KeywordCollection> GetKeywords(CancellationToken cancellationToken = default)
        {
            await using var fs = await (await FileProvider.GetFileInfo(Paths.GetKeywordMetadata(RootPath)).ConfigureAwait(false)).CreateReadStream().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<KeywordCollection>(fs, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return result ?? throw new System.Exception("Keywords are null");
        }
    }
}

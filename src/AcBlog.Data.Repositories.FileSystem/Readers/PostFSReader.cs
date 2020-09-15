using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.Searchers.Local;
using StardustDL.Extensions.FileProviders;
using System.Collections.Generic;
using System.Linq;
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

        protected override IAsyncEnumerable<string>? EfficientQuery(PostQueryRequest query, CancellationToken cancellationToken = default)
        {
            /*
            query.Pagination ??= new Pagination();

            var paging = PagingProvider;

            if (query.Type is not null)
            {
                switch (query.Type)
                {
                    case PostType.Article:
                        paging = new PagingProvider<string>(Paths.GetArticleRoot(RootPath), FileProvider);
                        break;
                    case PostType.Slides:
                        paging = new PagingProvider<string>(Paths.GetSlidesRoot(RootPath), FileProvider);
                        break;
                    case PostType.Note:
                        paging = new PagingProvider<string>(Paths.GetNoteRoot(RootPath), FileProvider);
                        break;
                }
            }
            else if (query.Category is not null && query.Category.Items.Any())
            {
                paging = new PagingProvider<string>(Paths.GetCategoryRoot(RootPath, query.Category), FileProvider);
            }
            else if (query.Keywords is not null && query.Keywords.Items.Any())
            {
                paging = new PagingProvider<string>(Paths.GetKeywordRoot(RootPath, query.Keywords), FileProvider);
            }

            await paging.FillPagination(query.Pagination);

            var res = new QueryResponse<string>(
                await paging.GetPaging(query.Pagination),
                query.Pagination);
            return res;
            */
            return base.EfficientQuery(query, cancellationToken);
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

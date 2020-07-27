using AcBlog.Data.Documents;
using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem.Readers;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Sdk;
using AcBlog.Tools.Sdk.Repositories;
using StardustDL.Extensions.FileProviders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Repositories
{
    internal class PostService : RecordRepoBaseService<Post, string, PostQueryRequest, IPostRepository>, IPostService
    {
        public PostService(IBlogService blog, string rootPath) : base(blog, new PostFSRepo(rootPath, new DocumentProtector()))
        {
            Searcher = Repository.CreateLocalSearcher();
        }

        public IProtector<Document> Protector => (Repository as PostFSRepo)!.Protector;

        public IPostRepositorySearcher Searcher { get; }

        public Task<CategoryTree> GetCategories(CancellationToken cancellationToken = default) => Repository.GetCategories(cancellationToken);

        public Task<KeywordCollection> GetKeywords(CancellationToken cancellationToken = default) => Repository.GetKeywords(cancellationToken);
    }
}

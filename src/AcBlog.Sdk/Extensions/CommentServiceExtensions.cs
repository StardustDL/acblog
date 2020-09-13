using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;

namespace AcBlog.Sdk.Extensions
{
    public static class CommentServiceExtensions
    {
        public static ICommentService AsService(this ICommentRepository repository, IBlogService blogService)
        {
            return new RepoBasedService(blogService, repository);
        }

        class RepoBasedService : RecordRepoBaseService<Comment, string, CommentQueryRequest, ICommentRepository>, ICommentService
        {
            public RepoBasedService(IBlogService blogService, ICommentRepository repository) : base(blogService, repository)
            {
                Searcher = Repository.CreateLocalSearcher();
            }

            public ICommentRepositorySearcher Searcher { get; }
        }
    }
}

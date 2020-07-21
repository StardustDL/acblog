using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;

namespace AcBlog.Sdk
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
            }
        }
    }
}

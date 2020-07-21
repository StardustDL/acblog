using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System.Threading.Tasks;

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
            }
        }

        public static string GetCommentUri(this Post value)
        {
            return $"posts/{value.Id}";
        }

        public static string GetCommentUri(this Page value)
        {
            return $"pages/{value.Id}";
        }
    }
}

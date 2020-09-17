using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;

namespace AcBlog.Services.Extensions
{
    public static class FileServiceExtensions
    {
        public static IFileService AsService(this IFileRepository repository, IBlogService blogService)
        {
            return new RepoBasedService(blogService, repository);
        }

        class RepoBasedService : RecordRepoBasedService<File, string, FileQueryRequest, IFileRepository>, IFileService
        {
            public RepoBasedService(IBlogService blogService, IFileRepository repository) : base(blogService, repository)
            {
            }
        }
    }
}

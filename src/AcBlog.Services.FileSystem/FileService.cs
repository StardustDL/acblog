using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem.Readers;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Services;
using StardustDL.Extensions.FileProviders;

namespace AcBlog.Services.FileSystem
{
    internal class FileService : RecordRepoBasedService<File, string, FileQueryRequest, IFileRepository>, IFileService
    {
        public FileService(IBlogService blog, string rootPath, IFileProvider fileProvider) : base(blog, new FileFSReader(rootPath, fileProvider))
        {
        }
    }
}

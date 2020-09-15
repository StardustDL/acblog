using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem.Readers;
using AcBlog.Data.Repositories.Searchers;
using StardustDL.Extensions.FileProviders;

namespace AcBlog.Sdk.FileSystem
{
    internal class FileService : RecordRepoBaseService<File, string, FileQueryRequest, IFileRepository>, IFileService
    {
        public FileService(IBlogService blog, string rootPath, IFileProvider fileProvider) : base(blog, new FileFSReader(rootPath, fileProvider))
        {
        }
    }
}

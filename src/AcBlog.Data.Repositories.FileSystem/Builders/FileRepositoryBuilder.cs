using AcBlog.Data.Models;

namespace AcBlog.Data.Repositories.FileSystem.Builders
{
    public class FileRepositoryBuilder : RecordRepositoryBuilderBase<File, string>
    {
        public FileRepositoryBuilder(string rootPath) : base(rootPath)
        {
        }
    }
}

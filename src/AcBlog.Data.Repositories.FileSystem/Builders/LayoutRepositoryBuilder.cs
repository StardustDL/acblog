using AcBlog.Data.Models;

namespace AcBlog.Data.Repositories.FileSystem.Builders
{
    public class LayoutRepositoryBuilder : RecordRepositoryBuilderBase<Layout, string>
    {
        public LayoutRepositoryBuilder(string rootPath) : base(rootPath)
        {
        }
    }
}

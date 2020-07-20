using AcBlog.Data.Models;

namespace AcBlog.Data.Repositories.FileSystem.Builders
{
    public class LayoutRepositoryBuilder : RecoderRepositoryBuilderBase<Layout, string>
    {
        public LayoutRepositoryBuilder(string rootPath) : base(rootPath)
        {
        }
    }
}

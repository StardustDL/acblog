using AcBlog.Data.Models;

namespace AcBlog.Data.Repositories.FileSystem.Builders
{
    public class PageRepositoryBuilder : RecoderRepositoryBuilderBase<Page, string>
    {
        public PageRepositoryBuilder(string rootPath) : base(rootPath)
        {
        }
    }
}

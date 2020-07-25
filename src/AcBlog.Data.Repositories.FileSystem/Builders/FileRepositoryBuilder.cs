using AcBlog.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Builders
{
    public class FileRepositoryBuilder : RecoderRepositoryBuilderBase<File, string>
    {
        public FileRepositoryBuilder(string rootPath) : base(rootPath)
        {
        }
    }
}

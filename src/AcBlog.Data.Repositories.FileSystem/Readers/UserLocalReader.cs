using System.IO;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public class UserLocalReader : UserReaderBase
    {
        public UserLocalReader(string rootPath) : base(rootPath)
        {
        }

        public override Task<bool> Exists(string id)
        {
            return Task.FromResult(File.Exists(GetUserPath(id)));
        }

        protected override Task<Stream> GetFileReadStream(string path)
        {
            return Task.FromResult<Stream>(File.Open(path, FileMode.Open, FileAccess.Read));
        }
    }
}

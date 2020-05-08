using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public class UserRemoteReader : UserReaderBase
    {
        public UserRemoteReader(string rootPath, HttpClient client) : base(rootPath)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public override async Task<bool> Exists(string id)
        {
            var rep = await Client.GetAsync(GetPath(id));
            return rep.IsSuccessStatusCode;
        }

        protected override async Task<Stream> GetFileReadStream(string path)
        {
            var rep = await Client.GetAsync(path);
            return await rep.Content.ReadAsStreamAsync();
        }
    }
}

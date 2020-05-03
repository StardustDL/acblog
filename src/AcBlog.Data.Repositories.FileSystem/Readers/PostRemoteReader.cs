using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public class PostRemoteReader : PostReaderBase
    {
        public PostRemoteReader(string rootPath, HttpClient client) : base(rootPath)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public override async Task<bool> Exists(string id)
        {
            var rep = await Client.GetAsync(GetPostPath(id));
            return rep.IsSuccessStatusCode;
        }

        protected override async Task<Stream> GetFileReadStream(string path)
        {
            var rep = await Client.GetAsync(path);
            rep.EnsureSuccessStatusCode();
            return await rep.Content.ReadAsStreamAsync();
        }
    }
}

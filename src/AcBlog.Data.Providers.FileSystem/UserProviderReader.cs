using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcBlog.Data.Providers.FileSystem
{
    public class UserProviderReader : UserProviderBase
    {
        public UserProviderReader(string rootPath, HttpClient client) : base(rootPath)
        {
            Client = client;
        }

        public override bool IsReadable => true;

        public override bool IsWritable => false;

        public HttpClient Client { get; }

        protected override async Task<bool> GetFileExists(string path)
        {
            var rep = await Client.GetAsync(path);
            return rep.IsSuccessStatusCode;
        }

        protected override async Task<Stream> GetFileReadStream(string path)
        {
            var rep = await Client.GetAsync(path);
            return await rep.Content.ReadAsStreamAsync();
        }

        protected override Task<Stream> GetFileWriteStream(string path) => throw new System.InvalidOperationException();
    }
}

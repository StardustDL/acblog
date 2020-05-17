using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public class CategoryRemoteReader : CategoryReaderBase
    {
        public CategoryRemoteReader(string rootPath, HttpClient client) : base(rootPath)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public override async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            var rep = await Client.GetAsync(GetPath(id),cancellationToken);
            return rep.IsSuccessStatusCode;
        }

        protected override async Task<Stream> GetFileReadStream(string path, CancellationToken cancellationToken = default)
        {
            var rep = await Client.GetAsync(path, cancellationToken);
            return await rep.Content.ReadAsStreamAsync();
        }
    }
}

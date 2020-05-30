using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class CategoryReaderBase : ReaderBase<Category, string, CategoryQueryRequest>, ICategoryRepository
    {
        protected CategoryReaderBase(string rootPath) : base(rootPath)
        {
        }
    }

    public class CategoryRemoteReader : CategoryReaderBase
    {
        public CategoryRemoteReader(string rootPath, HttpClient client) : base(rootPath)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public override async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            var rep = await Client.GetAsync(GetPath(id), cancellationToken);
            return rep.IsSuccessStatusCode;
        }

        protected override async Task<Stream> GetFileReadStream(string path, CancellationToken cancellationToken = default)
        {
            var rep = await Client.GetAsync(path, cancellationToken);
            return await rep.Content.ReadAsStreamAsync();
        }
    }

    public class CategoryLocalReader : CategoryReaderBase
    {
        public CategoryLocalReader(string rootPath) : base(rootPath)
        {
        }

        public override Task<bool> Exists(string id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(File.Exists(GetPath(id)));
        }

        protected override Task<Stream> GetFileReadStream(string path, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Stream>(File.Open(path, FileMode.Open, FileAccess.Read));
        }
    }
}

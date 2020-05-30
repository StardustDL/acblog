using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class UserReaderBase : ReaderBase<User, string, UserQueryRequest>, IUserRepository
    {
        protected UserReaderBase(string rootPath) : base(rootPath)
        {
        }
    }

    public class UserLocalReader : UserReaderBase
    {
        public UserLocalReader(string rootPath) : base(rootPath)
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

    public class UserRemoteReader : UserReaderBase
    {
        public UserRemoteReader(string rootPath, HttpClient client) : base(rootPath)
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
}

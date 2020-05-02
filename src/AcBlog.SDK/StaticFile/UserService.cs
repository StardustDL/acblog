using AcBlog.Data.Models;
using AcBlog.Data.Providers;
using AcBlog.Data.Providers.FileSystem;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcBlog.SDK.StaticFile
{
    internal class UserService : IUserService
    {
        UserProviderReader Reader { get; }

        public UserService(HttpClient httpClient)
        {
            HttpClient = httpClient;
            Reader = new UserProviderReader("/users", httpClient);
        }

        public HttpClient HttpClient { get; }

        public bool IsReadable => Reader.IsReadable;

        public bool IsWritable => Reader.IsWritable;

        public ProviderContext? Context { get => Reader.Context; set => Reader.Context = value; }

        public Task<IEnumerable<User>> All() => Reader.All();

        public Task<string?> Create(User value) => Reader.Create(value);

        public Task<bool> Delete(string id) => Reader.Delete(id);

        public Task<bool> Exists(string id) => Reader.Exists(id);

        public Task<User?> Get(string id) => Reader.Get(id);

        public Task<bool> Update(User value) => Reader.Update(value);
    }
}

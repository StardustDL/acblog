using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem.Readers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcBlog.SDK.StaticFile
{
    internal class PostService : IPostService
    {
        PostRemoteReader Reader { get; }

        public PostService(IBlogService blog, string rootPath, HttpClient httpClient)
        {
            Blog = blog;
            HttpClient = httpClient;
            Reader = new PostRemoteReader($"{rootPath}/posts", httpClient);
        }

        public HttpClient HttpClient { get; }

        public Task<bool> CanRead() => Reader.CanRead();

        public Task<bool> CanWrite() => Reader.CanWrite();

        public RepositoryAccessContext? Context { get => Reader.Context; set => Reader.Context = value; }

        public IBlogService Blog { get; private set; }

        public Task<IEnumerable<string>> All() => Reader.All();

        public Task<string?> Create(Post value) => Reader.Create(value);

        public Task<bool> Delete(string id) => Reader.Delete(id);

        public Task<bool> Exists(string id) => Reader.Exists(id);

        public Task<Post?> Get(string id) => Reader.Get(id);

        public Task<QueryResponse<string>> Query(PostQueryRequest query) => Reader.Query(query);

        public Task<bool> Update(Post value) => Reader.Update(value);
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Providers;
using AcBlog.Data.Providers.FileSystem;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcBlog.SDK.StaticFile
{
    internal class PostService : IPostService
    {
        PostProviderReader Reader { get; }

        public PostService(HttpClient httpClient)
        {
            HttpClient = httpClient;
            Reader = new PostProviderReader("/posts", httpClient);
        }

        public HttpClient HttpClient { get; }

        public bool IsReadable => Reader.IsReadable;

        public bool IsWritable => Reader.IsWritable;

        public ProviderContext? Context { get => Reader.Context; set => Reader.Context = value; }

        public Task<IEnumerable<Post>> All() => Reader.All();

        public Task<string?> Create(Post value) => Reader.Create(value);

        public Task<bool> Delete(string id) => Reader.Delete(id);

        public Task<bool> Exists(string id) => Reader.Exists(id);

        public Task<Post?> Get(string id) => Reader.Get(id);

        public Task<PostQueryResponse> Query(PostQueryRequest query) => Reader.Query(query);

        public Task<bool> Update(Post value) => Reader.Update(value);
    }
}

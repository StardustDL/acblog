using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem.Readers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcBlog.SDK.StaticFile
{
    internal class CategoryService : ICategoryService
    {
        CategoryRemoteReader Reader { get; }

        public CategoryService(IBlogService blog, string rootPath, HttpClient httpClient)
        {
            Blog = blog;
            HttpClient = httpClient;
            Reader = new CategoryRemoteReader(rootPath, httpClient);
        }

        public IBlogService Blog { get; private set; }

        public HttpClient HttpClient { get; }

        public Task<bool> CanRead() => Reader.CanRead();

        public Task<bool> CanWrite() => Reader.CanWrite();

        public RepositoryAccessContext? Context { get => Reader.Context; set => Reader.Context = value; }

        public Task<IEnumerable<string>> All() => Reader.All();

        public Task<string?> Create(Category value) => Reader.Create(value);

        public Task<bool> Delete(string id) => Reader.Delete(id);

        public Task<bool> Exists(string id) => Reader.Exists(id);

        public Task<Category?> Get(string id) => Reader.Get(id);

        public Task<bool> Update(Category value) => Reader.Update(value);

        public Task<QueryResponse<string>> Query(CategoryQueryRequest query) => Reader.Query(query);
    }
}

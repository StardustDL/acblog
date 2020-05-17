using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem.Readers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.SDK.StaticFile
{
    internal class KeywordService : IKeywordService
    {
        KeywordRemoteReader Reader { get; }

        public KeywordService(IBlogService blog, string rootPath, HttpClient httpClient)
        {
            BlogService = blog;
            HttpClient = httpClient;
            Reader = new KeywordRemoteReader(rootPath, httpClient);
        }

        public IBlogService BlogService { get; private set; }

        public HttpClient HttpClient { get; }

        public Task<bool> CanRead(CancellationToken cancellationToken = default) => Reader.CanRead(cancellationToken);

        public Task<bool> CanWrite(CancellationToken cancellationToken = default) => Reader.CanWrite(cancellationToken);

        public RepositoryAccessContext? Context { get => Reader.Context; set => Reader.Context = value; }

        public Task<IEnumerable<string>> All(CancellationToken cancellationToken = default) => Reader.All(cancellationToken);

        public Task<string?> Create(Keyword value, CancellationToken cancellationToken = default) => Reader.Create(value, cancellationToken);

        public Task<bool> Delete(string id, CancellationToken cancellationToken = default) => Reader.Delete(id, cancellationToken);

        public Task<bool> Exists(string id, CancellationToken cancellationToken = default) => Reader.Exists(id, cancellationToken);

        public Task<Keyword?> Get(string id, CancellationToken cancellationToken = default) => Reader.Get(id, cancellationToken);

        public Task<bool> Update(Keyword value, CancellationToken cancellationToken = default) => Reader.Update(value, cancellationToken);

        public Task<QueryResponse<string>> Query(KeywordQueryRequest query, CancellationToken cancellationToken = default) => Reader.Query(query, cancellationToken);
    }
}

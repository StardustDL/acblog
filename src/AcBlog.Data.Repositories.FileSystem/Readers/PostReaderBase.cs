using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class PostReaderBase : IPostRepository
    {
        public PostReaderBase(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        public RepositoryAccessContext? Context { get; set; }

        protected PostRepositoryConfig? Config { get; set; } = null;

        protected string GetPostPath(string id) => Path.Join(RootPath, $"{id}.json").Replace("\\", "/");

        protected async Task EnsureConfig()
        {
            if (Config != null)
                return;
            string path = Path.Join(RootPath, $"config.json");
            using var fs = await GetFileReadStream(path);
            Config = await JsonSerializer.DeserializeAsync<PostRepositoryConfig>(fs);
            if (Config.CountPerPage == 0)
                Config.CountPerPage = 10;
        }

        protected string GetPagePath(int number) => Path.Join(RootPath, "pages", $"{number}.json").Replace("\\", "/");

        protected abstract Task<Stream> GetFileReadStream(string path);

        public Task<bool> CanRead() => Task.FromResult(true);

        public Task<bool> CanWrite() => Task.FromResult(false);

        public Task<string?> Create(Post value) => Task.FromResult<string?>(null);

        public Task<bool> Delete(string id) => Task.FromResult(false);

        public abstract Task<bool> Exists(string id);

        public virtual async Task<Post?> Get(string id)
        {
            using var fs = await GetFileReadStream(GetPostPath(id));
            var result = await JsonSerializer.DeserializeAsync<Post>(fs);

            result.Id = id;
            return result;
        }

        public virtual Task<bool> Update(Post value) => Task.FromResult(false);

        public virtual async Task<IEnumerable<string>> All()
        {
            List<string> result = new List<string>();
            PostQueryRequest pq = new PostQueryRequest
            {
                Paging = new Pagination()
            };
            while (true)
            {
                var req = await Query(pq);
                result.AddRange(req.Results);
                if (!req.CurrentPage.HasNextPage)
                    break;
                pq.Paging = req.CurrentPage.NextPage();
            }
            return result;
        }

        public virtual async Task<QueryResponse<string>> Query(PostQueryRequest query)
        {
            if (query.Paging == null)
            {
                return new QueryResponse<string>();
            }
            await EnsureConfig();
            IList<string> result = Array.Empty<string>();
            try
            {
                using var fs = await GetFileReadStream(GetPagePath(query.Paging.PageNumber));
                result = await JsonSerializer.DeserializeAsync<IList<string>>(fs);
            }
            catch
            {
                return new QueryResponse<string>();
            }
            var res = new QueryResponse<string>(result.ToArray(), new Pagination
            {
                CountPerPage = Config!.CountPerPage,
                TotalCount = Config.TotalCount,
                PageNumber = query.Paging.PageNumber,
            });
            return res;
        }
    }
}

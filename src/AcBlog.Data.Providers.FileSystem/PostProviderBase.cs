using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Providers.FileSystem
{
    public abstract class PostProviderBase : IPostProvider
    {
        protected class PostListItem
        {
            public string Id { get; set; } = string.Empty;
        }

        public PostProviderBase(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        public ProviderContext? Context { get; set; }

        protected string GetPostPath(string id) => Path.Join(RootPath, $"{id}.json");

        protected string GetListPath() => Path.Join(RootPath, "list.json");

        protected abstract Task<Stream> GetFileReadStream(string path);

        protected abstract Task<Stream> GetFileWriteStream(string path);

        protected abstract Task<bool> GetFileExists(string path);

        protected async Task<IEnumerable<PostListItem>> GetList()
        {
            if (!IsReadable)
                return Array.Empty<PostListItem>();

            var path = GetListPath();
            if (!await GetFileExists(path))
                return Array.Empty<PostListItem>();
            using var fs = await GetFileReadStream(path);
            return await JsonSerializer.DeserializeAsync<IEnumerable<PostListItem>>(fs);
        }

        protected async Task UpdateList(IEnumerable<PostListItem> items)
        {
            var path = GetListPath();
            using var fs = await GetFileWriteStream(path);
            await JsonSerializer.SerializeAsync(fs, items);
        }

        public abstract bool IsReadable { get; }

        public abstract bool IsWritable { get; }

        public virtual async Task<string?> Create(Post value)
        {
            if (!IsWritable)
                return null;

            var id = Guid.NewGuid().ToString();
            value.Id = id;

            try
            {
                using var fs = await GetFileWriteStream(GetPostPath(id));
                await JsonSerializer.SerializeAsync(fs, value);
                await UpdateList((await GetList()).Concat(new[] { new PostListItem { Id = id } }));
            }
            catch
            {
                return null;
            }

            return id;
        }

        public virtual async Task<bool> Delete(string id)
        {
            if (!IsWritable)
                return false;
            await UpdateList(from x in await GetList() where x.Id != id select x);
            return true;
        }

        public virtual async Task<bool> Exists(string id)
        {
            if (!IsReadable)
                return false;
            return (await GetList()).Any(x => x.Id == id);
        }

        public virtual async Task<Post?> Get(string id)
        {
            if (!IsReadable)
                return null;

            using var fs = await GetFileReadStream(GetPostPath(id));

            var result = await JsonSerializer.DeserializeAsync<Post>(fs);

            result.Id = id;
            return result;
        }

        public virtual async Task<bool> Update(Post value)
        {
            if (!IsWritable)
                return false;

            if (await Exists(value.Id))
            {
                using var fs = await GetFileWriteStream(GetPostPath(value.Id));
                await JsonSerializer.SerializeAsync(fs, value);
                return true;
            }
            return false;
        }

        public virtual async Task<IEnumerable<Post>> All()
        {
            if (!IsReadable)
                return Array.Empty<Post>();
            List<Post> result = new List<Post>();
            foreach (var v in await GetList())
            {
                var item = await Get(v.Id);
                if (item == null)
                    continue;
                result.Add(item);
            }
            return result;
        }

        public virtual async Task<PostQueryResponse> Query(PostQueryRequest query)
        {
            var all = await All();
            var res = new PostQueryResponse
            {
                Results = all.ToArray()
            };
            return res;
        }
    }
}

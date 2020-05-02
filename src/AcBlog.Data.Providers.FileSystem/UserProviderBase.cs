using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Providers.FileSystem
{
    public abstract class UserProviderBase : IUserProvider
    {
        protected class UserListItem
        {
            public string Id { get; set; } = string.Empty;
        }

        public UserProviderBase(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        public ProviderContext? Context { get; set; }

        protected string GetUserPath(string id) => Path.Join(RootPath, $"{id}.json");

        protected string GetListPath() => Path.Join(RootPath, "list.json");

        protected abstract Task<Stream> GetFileReadStream(string path);

        protected abstract Task<Stream> GetFileWriteStream(string path);

        protected abstract Task<bool> GetFileExists(string path);

        protected async Task<IEnumerable<UserListItem>> GetList()
        {
            if (!IsReadable)
                return Array.Empty<UserListItem>();

            var path = GetListPath();
            if (!await GetFileExists(path))
                return Array.Empty<UserListItem>();
            using var fs = await GetFileReadStream(path);
            return await JsonSerializer.DeserializeAsync<IEnumerable<UserListItem>>(fs);
        }

        protected async Task UpdateList(IEnumerable<UserListItem> items)
        {
            var path = GetListPath();
            using var fs = await GetFileWriteStream(path);
            await JsonSerializer.SerializeAsync(fs, items);
        }

        public abstract bool IsReadable { get; }

        public abstract bool IsWritable { get; }

        public virtual async Task<string?> Create(User value)
        {
            if (!IsWritable)
                return null;

            var id = Guid.NewGuid().ToString();
            value.Id = id;

            try
            {
                using var fs = await GetFileWriteStream(GetUserPath(id));
                await JsonSerializer.SerializeAsync(fs, value);
                await UpdateList((await GetList()).Concat(new[] { new UserListItem { Id = id } }));
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

        public virtual async Task<User?> Get(string id)
        {
            if (!IsReadable)
                return null;

            using var fs = await GetFileReadStream(GetUserPath(id));

            var result = await JsonSerializer.DeserializeAsync<User>(fs);

            result.Id = id;
            return result;
        }

        public virtual async Task<bool> Update(User value)
        {
            if (!IsWritable)
                return false;

            if (await Exists(value.Id))
            {
                using var fs = await GetFileWriteStream(GetUserPath(value.Id));
                await JsonSerializer.SerializeAsync(fs, value);
                return true;
            }
            return false;
        }

        public virtual async Task<IEnumerable<User>> All()
        {
            if (!IsReadable)
                return Array.Empty<User>();
            List<User> result = new List<User>();
            foreach (var v in await GetList())
            {
                var item = await Get(v.Id);
                if (item == null)
                    continue;
                result.Add(item);
            }
            return result;
        }
    }
}

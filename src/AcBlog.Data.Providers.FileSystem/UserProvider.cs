using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Providers.FileSystem
{
    public class UserProvider : IUserProvider
    {
        public UserProvider(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        public bool IsReadable => true;

        public bool IsWritable => true;

        public ProviderContext Context { get; set; }

        string GetUserFile(string id) => Path.Join(RootPath, $"{id}.json");

        public async Task<string> Create(User value)
        {
            var id = Guid.NewGuid().ToString();
            value.Id = id;

            using var fs = File.OpenWrite(GetUserFile(id));
            await JsonSerializer.SerializeAsync(fs, value);

            return id;
        }

        public Task<bool> Delete(string id)
        {
            var file = GetUserFile(id);
            if (File.Exists(file))
            {
                File.Delete(file);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> Exists(string id)
        {
            return Task.FromResult(File.Exists(GetUserFile(id)));
        }

        public async Task<User> Get(string id)
        {
            using var fs = File.OpenRead(GetUserFile(id));
            var result = await JsonSerializer.DeserializeAsync<User>(fs);
            result.Id = id;
            return result;
        }

        public async Task<bool> Update(User value)
        {
            var file = GetUserFile(value.Id);
            if (File.Exists(file))
            {
                using var fs = File.OpenWrite(file);
                await JsonSerializer.SerializeAsync(fs, value);

                return true;
            }
            return false;
        }

        public async IAsyncEnumerable<User> All()
        {
            foreach (var v in Directory.GetFiles(RootPath, "*.json"))
            {
                yield return await Get(Path.GetFileNameWithoutExtension(v));
            }
        }
    }
}

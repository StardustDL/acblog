using AcBlog.Data.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Providers.FileSystem
{
    public class PostProvider : IPostProvider
    {
        public PostProvider(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        public bool IsReadable => true;

        public bool IsWritable => true;

        public ProviderContext Context { get; set; }

        string GetPostFile(string id) => Path.Join(RootPath, $"{id}.json");

        public async Task<string> Create(Post value)
        {
            var id = Guid.NewGuid().ToString();

            using var fs = File.OpenWrite(GetPostFile(id));
            await JsonSerializer.SerializeAsync(fs, value);

            return id;
        }

        public Task<bool> Delete(string id)
        {
            var file = GetPostFile(id);
            if (File.Exists(file))
            {
                File.Delete(file);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> Exists(string id)
        {
            return Task.FromResult(File.Exists(GetPostFile(id)));
        }

        public async Task<Post> Get(string id)
        {
            using var fs = File.OpenRead(GetPostFile(id));
            var result = await JsonSerializer.DeserializeAsync<Post>(fs);
            result.Id = id;
            return result;
        }

        public async Task<bool> Update(Post value)
        {
            var file = GetPostFile(value.Id);
            if (File.Exists(file))
            {
                using var fs = File.OpenWrite(file);
                await JsonSerializer.SerializeAsync(fs, value);

                return true;
            }
            return false;
        }
    }
}

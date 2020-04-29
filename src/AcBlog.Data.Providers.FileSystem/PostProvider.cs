using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
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

        public ProviderContext? Context { get; set; }

        string GetPostFile(string id) => Path.Join(RootPath, $"{id}.json");

        async Task SaveFile(FileStream stream, Post data)
        {
            await JsonSerializer.SerializeAsync(stream, data);
        }

        async Task<Post> LoadFile(FileStream stream)
        {
            var result = await JsonSerializer.DeserializeAsync<Post>(stream);
            return result;
        }

        public async Task<string?> Create(Post value)
        {
            var id = Guid.NewGuid().ToString();
            value.Id = id;

            try
            {
                using var fs = File.Open(GetPostFile(id), FileMode.CreateNew, FileAccess.Write);
                await SaveFile(fs, value);
            }
            catch
            {
                return null;
            }

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
            using var fs = File.Open(GetPostFile(id), FileMode.Open, FileAccess.Read);

            var result = await LoadFile(fs);

            result.Id = id;
            return result;
        }

        public async Task<bool> Update(Post value)
        {
            var file = GetPostFile(value.Id);
            if (File.Exists(file))
            {
                using var fs = File.Open(file, FileMode.Truncate, FileAccess.Write);
                await SaveFile(fs, value);

                return true;
            }
            return false;
        }

        public async IAsyncEnumerable<Post> All()
        {
            foreach (var v in Directory.GetFiles(RootPath, "*.json"))
            {
                yield return await Get(Path.GetFileNameWithoutExtension(v));
            }
        }
    }
}

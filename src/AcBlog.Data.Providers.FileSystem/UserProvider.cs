using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Providers.FileSystem
{
    public class UserProvider : UserProviderBase
    {
        public UserProvider(string rootPath) : base(rootPath)
        {
        }

        public override bool IsReadable => true;

        public override bool IsWritable => true;

        protected override Task<Stream> GetFileReadStream(string path)
        {
            return Task.FromResult<Stream>(File.Open(path, FileMode.Open, FileAccess.Read));
        }

        protected override Task<Stream> GetFileWriteStream(string path)
        {
            if (File.Exists(path))
                return Task.FromResult<Stream>(File.Open(path, FileMode.Truncate, FileAccess.Write));
            else
                return Task.FromResult<Stream>(File.Open(path, FileMode.CreateNew, FileAccess.Write));
        }

        protected override Task<bool> GetFileExists(string path) => Task.FromResult(File.Exists(path));

        public override async Task<bool> Delete(string id)
        {
            var file = GetUserPath(id);
            if (File.Exists(file))
            {
                File.Delete(file);
                await base.Delete(id);
                return true;
            }
            return false;
        }

        public override Task<bool> Exists(string id) => GetFileExists(GetUserPath(id));
    }
}

using System.IO;
using System;

namespace AcBlog.Data.Repositories.FileSystem
{
    public class FSBuilder
    {
        public FSBuilder(string rootPath) => RootPath = rootPath;

        public string RootPath { get; }

        public void EnsureDirectoryExists(string subpath = "", bool isExists = true)
        {
            string path = Path.Join(RootPath, subpath);
            if (isExists)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            else
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
        }

        public void EnsureDirectoryEmpty(string subpath = "")
        {
            string path = Path.Join(RootPath, subpath);
            EnsureDirectoryExists(subpath, false);
            Directory.CreateDirectory(path);
        }

        public void EnsureFileExists(string subpath, bool isExists = true, byte[]? initialData = null)
        {
            string path = Path.Join(RootPath, subpath);
            if (isExists)
            {
                if (!File.Exists(path))
                {
                    EnsureDirectoryExists(Path.GetDirectoryName(subpath));
                    File.WriteAllBytes(path, initialData ?? Array.Empty<byte>());
                }
            }
            else
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        public void EnsureFileEmpty(string subpath)
        {
            string path = Path.Join(RootPath, subpath);
            EnsureFileExists(subpath, false);
            EnsureFileExists(subpath, true);
        }

        public Stream GetFileRewriteStream(string subpath)
        {
            string path = Path.Join(RootPath, subpath);
            EnsureFileEmpty(subpath);
            return File.OpenWrite(path);
        }

        public FSBuilder CreateSubDirectoryBuilder(string subpath)
        {
            string path = Path.Join(RootPath, subpath);
            return new FSBuilder(path);
        }
    }
}

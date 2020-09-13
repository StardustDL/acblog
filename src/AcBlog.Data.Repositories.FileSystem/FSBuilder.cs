using System;
using System.IO;

namespace AcBlog.Data.Repositories.FileSystem
{
    public static class FSStaticBuilder
    {
        public static void EnsureDirectoryExists(string path, bool isExists = true)
        {
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

        public static void EnsureDirectoryEmpty(string path)
        {
            EnsureDirectoryExists(path);
            foreach (var v in Directory.GetFiles(path))
            {
                File.Delete(v);
            }
            foreach (var v in Directory.GetDirectories(path))
            {
                Directory.Delete(v, true);
            }
        }

        public static void EnsureFileExists(string path, bool isExists = true, byte[]? initialData = null)
        {
            if (isExists)
            {
                if (!File.Exists(path))
                {
                    var par = Path.GetDirectoryName(path);
                    if (!string.IsNullOrWhiteSpace(par))
                        EnsureDirectoryExists(par);
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

        public static void EnsureFileEmpty(string path)
        {
            EnsureFileExists(path, false);
            EnsureFileExists(path, true);
        }

        public static Stream GetFileRewriteStream(string path)
        {
            EnsureFileEmpty(path);
            return File.OpenWrite(path);
        }
    }

    public class FSBuilder
    {
        public FSBuilder(string rootPath) => RootPath = rootPath;

        public string RootPath { get; }

        public void EnsureDirectoryExists(string subpath = "", bool isExists = true)
        {
            string path = Path.Join(RootPath, subpath);
            FSStaticBuilder.EnsureDirectoryExists(path, isExists);
        }

        public void EnsureDirectoryEmpty(string subpath = "")
        {
            string path = Path.Join(RootPath, subpath);
            FSStaticBuilder.EnsureDirectoryEmpty(path);
        }

        public void EnsureFileExists(string subpath, bool isExists = true, byte[]? initialData = null)
        {
            string path = Path.Join(RootPath, subpath);
            FSStaticBuilder.EnsureFileExists(path, isExists, initialData);
        }

        public void EnsureFileEmpty(string subpath)
        {
            string path = Path.Join(RootPath, subpath);
            FSStaticBuilder.EnsureFileEmpty(path);
        }

        public Stream GetFileRewriteStream(string subpath)
        {
            string path = Path.Join(RootPath, subpath);
            return FSStaticBuilder.GetFileRewriteStream(path);
        }

        public FSBuilder CreateSubDirectoryBuilder(string subpath)
        {
            string path = Path.Join(RootPath, subpath);
            return new FSBuilder(path);
        }
    }
}

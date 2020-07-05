using Microsoft.Extensions.FileProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StardustDL.Extensions.FileProviders
{
    public interface IFileProvider
    {
        Task<IFileInfo> GetFileInfo(string subpath);

        Task<IDirectoryContents> GetDirectoryContents(string subpath);
    }

    public interface IDirectoryContents
    {
        Task<bool> Exists();

        IAsyncEnumerable<IFileInfo> Children();
    }

    public interface IFileInfo
    {
        Task<bool> Exists();

        Task<long> Length();

        string PhysicalPath { get; }

        string Name { get; }

        bool IsDirectory { get; }

        Task<Stream> CreateReadStream();
    }

    public static class FileProviderExtension
    {
        class MSFileProviderWrapper : IFileProvider
        {
            class MSFileInfoWrapper : IFileInfo
            {
                Microsoft.Extensions.FileProviders.IFileInfo fileInfo;

                public MSFileInfoWrapper(Microsoft.Extensions.FileProviders.IFileInfo fileInfo) => this.fileInfo = fileInfo;

                public string PhysicalPath => fileInfo.PhysicalPath;

                public string Name => fileInfo.Name;

                public bool IsDirectory => fileInfo.IsDirectory;

                public Task<Stream> CreateReadStream() => Task.FromResult(fileInfo.CreateReadStream());
                public Task<bool> Exists() => Task.FromResult(fileInfo.Exists);
                public Task<long> Length() => Task.FromResult(fileInfo.Length);
            }

            class MSDirectoryContentsWrapper : IDirectoryContents
            {
                Microsoft.Extensions.FileProviders.IDirectoryContents fileInfo;

                public MSDirectoryContentsWrapper(Microsoft.Extensions.FileProviders.IDirectoryContents fileInfo) => this.fileInfo = fileInfo;

                public async IAsyncEnumerable<IFileInfo> Children()
                {
                    foreach (var v in fileInfo)
                    {
                        yield return await Task.FromResult(new MSFileInfoWrapper(v));
                    }
                }

                public Task<bool> Exists() => Task.FromResult(fileInfo.Exists);
            }

            Microsoft.Extensions.FileProviders.IFileProvider fileInfo;

            public MSFileProviderWrapper(Microsoft.Extensions.FileProviders.IFileProvider fileInfo) => this.fileInfo = fileInfo;

            public Task<IFileInfo> GetFileInfo(string subpath) => Task.FromResult<IFileInfo>(new MSFileInfoWrapper(fileInfo.GetFileInfo(subpath)));

            public Task<IDirectoryContents> GetDirectoryContents(string subpath) => Task.FromResult<IDirectoryContents>(new MSDirectoryContentsWrapper(fileInfo.GetDirectoryContents(subpath)));
        }

        public static IFileProvider AsFileProvider(this Microsoft.Extensions.FileProviders.IFileProvider fileProvider) => new MSFileProviderWrapper(fileProvider);
    }
}

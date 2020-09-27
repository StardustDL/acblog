using System.Collections.Generic;
using System.IO;
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
                readonly Microsoft.Extensions.FileProviders.IFileInfo _fileInfo;

                public MSFileInfoWrapper(Microsoft.Extensions.FileProviders.IFileInfo fileInfo) => this._fileInfo = fileInfo;

                public string PhysicalPath => _fileInfo.PhysicalPath;

                public string Name => _fileInfo.Name;

                public bool IsDirectory => _fileInfo.IsDirectory;

                public Task<Stream> CreateReadStream() => Task.FromResult(_fileInfo.CreateReadStream());
                public Task<bool> Exists() => Task.FromResult(_fileInfo.Exists);
                public Task<long> Length() => Task.FromResult(_fileInfo.Length);
            }

            class MSDirectoryContentsWrapper : IDirectoryContents
            {
                readonly Microsoft.Extensions.FileProviders.IDirectoryContents _fileInfo;

                public MSDirectoryContentsWrapper(Microsoft.Extensions.FileProviders.IDirectoryContents fileInfo) => this._fileInfo = fileInfo;

                public async IAsyncEnumerable<IFileInfo> Children()
                {
                    foreach (var v in _fileInfo)
                    {
                        yield return await Task.FromResult(new MSFileInfoWrapper(v));
                    }
                }

                public Task<bool> Exists() => Task.FromResult(_fileInfo.Exists);
            }

            readonly Microsoft.Extensions.FileProviders.IFileProvider _fileInfo;

            public MSFileProviderWrapper(Microsoft.Extensions.FileProviders.IFileProvider fileInfo) => this._fileInfo = fileInfo;

            public Task<IFileInfo> GetFileInfo(string subpath) => Task.FromResult<IFileInfo>(new MSFileInfoWrapper(_fileInfo.GetFileInfo(subpath)));

            public Task<IDirectoryContents> GetDirectoryContents(string subpath) => Task.FromResult<IDirectoryContents>(new MSDirectoryContentsWrapper(_fileInfo.GetDirectoryContents(subpath)));
        }

        public static IFileProvider AsFileProvider(this Microsoft.Extensions.FileProviders.IFileProvider fileProvider) => new MSFileProviderWrapper(fileProvider);
    }
}

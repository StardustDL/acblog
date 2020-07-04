using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Net.Http;

namespace StardustDL.Extensions.FileProviders.Http
{
    public class HttpFileProvider : IFileProvider
    {
        public HttpFileProvider(HttpClient client) => Client = client;

        public HttpClient Client { get; }

        public IDirectoryContents GetDirectoryContents(string subpath) => throw new NotImplementedException();
        
        public IFileInfo GetFileInfo(string subpath)
        {
            var result = Client.GetAsync(subpath).ConfigureAwait(false).GetAwaiter().GetResult();
            return new HttpFileInfo(result);
        }

        public IChangeToken Watch(string filter) => throw new NotImplementedException();
    }
}

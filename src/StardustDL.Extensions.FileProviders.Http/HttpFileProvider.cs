using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StardustDL.Extensions.FileProviders.Http
{
    public class HttpFileProvider : IFileProvider
    {
        public HttpFileProvider(HttpClient client) => Client = client;

        public HttpClient Client { get; }

        public Task<IDirectoryContents> GetDirectoryContents(string subpath) => throw new NotImplementedException();

        public async Task<IFileInfo> GetFileInfo(string subpath)
        {
            var result = await Client.GetAsync(subpath).ConfigureAwait(false);
            return new HttpFileInfo(result);
        }
    }
}

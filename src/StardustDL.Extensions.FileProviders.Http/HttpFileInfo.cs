using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace StardustDL.Extensions.FileProviders.Http
{
    internal class HttpFileInfo : IFileInfo
    {
        private HttpResponseMessage Response { get; }

        public HttpFileInfo(HttpResponseMessage response)
        {
            Response = response;
        }

        public Task<bool> Exists() => Task.FromResult(Response.IsSuccessStatusCode);

        public Task<long> Length() => Task.FromResult(Response.Content.Headers.ContentLength ?? 0);

        public string PhysicalPath => Response.Content.Headers.ContentLocation.AbsoluteUri;

        public string Name => Response.Content.Headers.ContentLocation.LocalPath;

        // public DateTimeOffset LastModified => Response.Content.Headers.LastModified ?? DateTimeOffset.MinValue;

        public bool IsDirectory => false;

        public async Task<Stream> CreateReadStream()
        {
            return await Response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }
    }
}

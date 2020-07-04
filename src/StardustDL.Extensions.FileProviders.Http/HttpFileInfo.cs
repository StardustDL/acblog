using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Net.Http;

namespace StardustDL.Extensions.FileProviders.Http
{
    internal class HttpFileInfo : IFileInfo
    {
        private HttpResponseMessage Response { get; }

        public HttpFileInfo(HttpResponseMessage response)
        {
            Response = response;
        }

        public bool Exists => Response.IsSuccessStatusCode;

        public long Length => Response.Content.Headers.ContentLength ?? 0;

        public string PhysicalPath => Response.Content.Headers.ContentLocation.AbsoluteUri;

        public string Name => Response.Content.Headers.ContentLocation.LocalPath;

        public DateTimeOffset LastModified => Response.Content.Headers.LastModified ?? DateTimeOffset.MinValue;

        public bool IsDirectory => false;

        public Stream CreateReadStream()
        {
            return Response.Content.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}

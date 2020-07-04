using System;
using System.Net.Http;
using System.Text;

namespace AcBlog.Sdk.Api
{
    public class ApiBlogService : IBlogService
    {
        public ApiBlogService(HttpClient httpClient)
        {
            HttpClient = httpClient;

            PostService = new PostService(this, httpClient);
        }

        public HttpClient HttpClient { get; }

        public IPostService PostService { get; private set; }
    }
}

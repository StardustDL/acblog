using System;
using System.Net.Http;
using System.Text;

namespace AcBlog.SDK.API
{
    public class HttpApiBlogService : IBlogService
    {
        public HttpApiBlogService(HttpClient httpClient)
        {
            HttpClient = httpClient;

            UserService = new UserService(this, httpClient);
            PostService = new PostService(this, httpClient);
        }

        public HttpClient HttpClient { get; }

        public IUserService UserService { get; private set; }

        public IPostService PostService { get; private set; }
    }
}

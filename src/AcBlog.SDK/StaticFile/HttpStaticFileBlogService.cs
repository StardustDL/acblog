using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Providers;
using AcBlog.Data.Providers.FileSystem;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AcBlog.SDK.StaticFile
{
    public class HttpStaticFileBlogService : IBlogService
    {
        public HttpStaticFileBlogService(HttpClient httpClient)
        {
            HttpClient = httpClient;

            UserService = new UserService(httpClient);
            PostService = new PostService(httpClient);
        }

        public HttpClient HttpClient { get; }

        public IUserService UserService { get; private set; }

        public IPostService PostService { get; private set; }
    }
}

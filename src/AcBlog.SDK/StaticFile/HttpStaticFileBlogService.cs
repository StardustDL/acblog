using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
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
        public HttpStaticFileBlogService(string rootPath, HttpClient httpClient)
        {
            HttpClient = httpClient;

            UserService = new UserService(this, $"{rootPath}/users", httpClient);
            ArticleService = new PostService(this, $"{rootPath}/articles", httpClient);
            SlidesService = new PostService(this, $"{rootPath}/slides", httpClient);
        }

        public HttpClient HttpClient { get; }

        public IUserService UserService { get; private set; }

        public IPostService ArticleService { get; private set; }

        public IPostService SlidesService { get; private set; }
    }
}

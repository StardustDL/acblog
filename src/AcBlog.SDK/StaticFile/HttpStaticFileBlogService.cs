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
            PostService = new PostService(this, $"{rootPath}/posts", httpClient);
            CategoryService = new CategoryService(this, $"{rootPath}/categories", httpClient);
            KeywordService = new KeywordService(this, $"{rootPath}/keywords", httpClient);
        }

        public HttpClient HttpClient { get; }

        public IUserService UserService { get; private set; }

        public ICategoryService CategoryService { get; private set; }

        public IKeywordService KeywordService { get; private set; }

        public IPostService PostService { get; private set; }
    }
}

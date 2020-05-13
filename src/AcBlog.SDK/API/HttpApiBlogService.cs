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
            CategoryService = new CategoryService(this, httpClient);
            KeywordService = new KeywordService(this, httpClient);
        }

        public HttpClient HttpClient { get; }

        public IUserService UserService { get; private set; }

        public IPostService PostService { get; private set; }

        public IKeywordService KeywordService { get; private set; }

        public ICategoryService CategoryService { get; private set; }
    }
}

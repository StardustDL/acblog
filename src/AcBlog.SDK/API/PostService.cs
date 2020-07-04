using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AcBlog.SDK.API
{
    internal class PostService : BaseApiService<Post, PostQueryRequest>, IPostService
    {
        public PostService(IBlogService blog, HttpClient httpClient) : base(blog, httpClient)
        {
            Protector = new DocumentProtector();
        }

        public IProtector<Document> Protector { get; }

        protected override string PrepUrl => "/Posts";
    }
}

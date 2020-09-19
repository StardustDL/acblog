using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Services;
using System.Net.Http;

namespace AcBlog.Sdk.Api
{
    internal class LayoutService : BaseRecordApiService<Layout, LayoutQueryRequest>, ILayoutService
    {
        public LayoutService(IBlogService blog, HttpClient httpClient) : base(blog, httpClient)
        {
        }

        protected override string PrepUrl => "/Layouts";
    }
}

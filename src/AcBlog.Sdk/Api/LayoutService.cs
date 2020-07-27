using AcBlog.Data.Documents;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories.Searchers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk.Api
{
    internal class LayoutService : BaseRecordApiService<Layout, LayoutQueryRequest, ILayoutRepositorySearcher>, ILayoutService
    {
        public LayoutService(IBlogService blog, HttpClient httpClient) : base(blog, httpClient)
        {
        }

        protected override string PrepUrl => "/Layouts";
    }
}

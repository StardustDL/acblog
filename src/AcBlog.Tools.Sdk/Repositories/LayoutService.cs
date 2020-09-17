using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Sdk;
using AcBlog.Services;

namespace AcBlog.Tools.Sdk.Repositories
{
    internal class LayoutService : RecordRepoBasedService<Layout, string, LayoutQueryRequest, ILayoutRepository>, ILayoutService
    {
        public LayoutService(IBlogService blog, string rootPath) : base(blog, new LayoutFSRepo(rootPath))
        {
        }
    }
}

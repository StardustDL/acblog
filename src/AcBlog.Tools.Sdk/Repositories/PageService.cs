using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Sdk;

namespace AcBlog.Tools.Sdk.Repositories
{
    internal class PageService : RecordRepoBaseService<Page, string, PageQueryRequest, IPageRepository>, IPageService
    {
        public PageService(IBlogService blog, string rootPath) : base(blog, new PageFSRepo(rootPath))
        {
        }
    }
}

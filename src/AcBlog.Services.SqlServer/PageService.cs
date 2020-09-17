using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Data.Repositories.SqlServer;
using AcBlog.Data.Repositories.SqlServer.Models;

namespace AcBlog.Services.SqlServer
{
    internal class PageService : RecordRepoBasedService<Page, string, PageQueryRequest, IPageRepository>, IPageService
    {
        public PageService(IBlogService blog, BlogDataContext dataContext) : base(blog, new PageDBRepository(dataContext))
        {
        }
    }
}

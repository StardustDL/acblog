using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Data.Repositories.SqlServer;
using AcBlog.Data.Repositories.SqlServer.Models;

namespace AcBlog.Services.SqlServer
{
    internal class LayoutService : RecordRepoBasedService<Layout, string, LayoutQueryRequest, ILayoutRepository>, ILayoutService
    {
        public LayoutService(IBlogService blog, BlogDataContext dataContext) : base(blog, new LayoutDBRepository(dataContext))
        {
        }
    }
}

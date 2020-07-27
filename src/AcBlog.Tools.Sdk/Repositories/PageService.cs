using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem.Readers;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Sdk;
using AcBlog.Tools.Sdk.Repositories;
using StardustDL.Extensions.FileProviders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Repositories
{
    internal class PageService : RecordRepoBaseService<Page, string, PageQueryRequest, IPageRepository>, IPageService
    {
        public PageService(IBlogService blog, string rootPath) : base(blog, new PageFSRepo(rootPath))
        {
            Searcher = Repository.CreateLocalSearcher();
        }

        public IPageRepositorySearcher Searcher { get; }
    }
}

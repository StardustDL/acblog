using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Data.Repositories.FileSystem.Readers;
using AcBlog.Data.Repositories.Searchers;
using StardustDL.Extensions.FileProviders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk.FileSystem
{
    internal class PageService : RecordRepoBaseService<Page, string, PageQueryRequest, IPageRepository>, IPageService
    {
        public PageService(IBlogService blog, string rootPath, IFileProvider fileProvider) : base(blog, new PageFSReader(rootPath, fileProvider))
        {
            Searcher = Repository.CreateLocalSearcher();
        }

        public IPageRepositorySearcher Searcher { get; }
    }
}

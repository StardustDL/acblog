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
    internal class LayoutService : RecordRepoBaseService<Layout, string, LayoutQueryRequest, ILayoutRepository>, ILayoutService
    {
        public LayoutService(IBlogService blog, string rootPath, IFileProvider fileProvider) : base(blog, new LayoutFSReader(rootPath, fileProvider))
        {
            Searcher = Repository.CreateLocalSearcher();
        }

        public ILayoutRepositorySearcher Searcher { get; }
    }
}

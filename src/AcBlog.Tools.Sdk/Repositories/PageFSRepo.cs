using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Tools.Sdk.Models.Text;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Repositories
{
    internal class PageFSRepo : RecordFSRepo<Page, PageQueryRequest, PageMetadata>, IPageRepository
    {
        public PageFSRepo(string rootPath) : base(rootPath)
        {
        }

        public override Task<QueryResponse<string>> Query(PageQueryRequest query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        protected override Task<Page> CreateExistedItem(string id, PageMetadata metadata, string content)
        {
            string path = GetPath(id);
            if (string.IsNullOrEmpty(metadata.id))
                metadata.id = id;
            if (string.IsNullOrEmpty(metadata.creationTime))
            {
                metadata.creationTime = File.GetCreationTime(path).ToString();
            }
            if (string.IsNullOrEmpty(metadata.modificationTime))
            {
                metadata.modificationTime = File.GetLastWriteTime(path).ToString();
            }

            Page result = new Page();
            metadata.ApplyTo(result);
            result.Content = content;

            return Task.FromResult(result);
        }

        protected override Task<(PageMetadata, string)> CreateNewItem(Page value)
        {
            return Task.FromResult((new PageMetadata(value), value.Content));
        }
    }
}

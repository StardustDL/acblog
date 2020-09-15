using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using AcBlog.Tools.Sdk.Models.Text;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Repositories
{
    internal class LayoutFSRepo : RecordFSRepo<Layout, LayoutQueryRequest, LayoutMetadata>, ILayoutRepository
    {
        public LayoutFSRepo(string rootPath) : base(rootPath)
        {
        }

        protected override Task<Layout> CreateExistedItem(string id, LayoutMetadata metadata, string content)
        {
            string path = GetPath(id);
            if (string.IsNullOrEmpty(metadata.id))
                metadata.id = id;
            if (string.IsNullOrEmpty(metadata.creationTime))
            {
                metadata.creationTime = System.IO.File.GetCreationTime(path).ToString();
            }
            if (string.IsNullOrEmpty(metadata.modificationTime))
            {
                metadata.modificationTime = System.IO.File.GetLastWriteTime(path).ToString();
            }

            Layout result = new Layout();
            metadata.ApplyTo(result);
            result.Template = content;

            return Task.FromResult(result);
        }

        protected override Task<(LayoutMetadata, string)> CreateNewItem(Layout value)
        {
            return Task.FromResult((new LayoutMetadata(value), value.Template));
        }
    }
}

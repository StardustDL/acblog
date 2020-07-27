using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories.Searchers
{
    public interface IFileRepositorySearcher : IRecordRepositorySearcher<File, string, FileQueryRequest, IFileRepository>
    {

    }
}

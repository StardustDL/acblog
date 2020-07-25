using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories
{
    public interface IFileRepository : IRecordRepository<File, string, FileQueryRequest>
    {
    }
}

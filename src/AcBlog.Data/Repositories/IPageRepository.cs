using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories
{
    public interface IPageRepository : IRecordRepository<Page, string, PageQueryRequest>
    {
    }
}

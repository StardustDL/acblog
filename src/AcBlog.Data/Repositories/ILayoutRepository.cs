using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories
{
    public interface ILayoutRepository : IRecordRepository<Layout, string, LayoutQueryRequest>
    {
    }
}

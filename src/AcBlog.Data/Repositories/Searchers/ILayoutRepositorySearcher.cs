using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories.Searchers
{
    public interface ILayoutRepositorySearcher : IRecordRepositorySearcher<Layout, string, LayoutQueryRequest, ILayoutRepository>
    {

    }
}

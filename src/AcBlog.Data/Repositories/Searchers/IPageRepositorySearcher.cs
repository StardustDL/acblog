using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories.Searchers
{
    public interface IPageRepositorySearcher : IRecordRepositorySearcher<Page, string, PageQueryRequest, IPageRepository>
    {

    }
}

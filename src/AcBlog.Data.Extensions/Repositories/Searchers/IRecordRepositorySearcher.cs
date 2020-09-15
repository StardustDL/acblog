using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers
{
    public interface IRecordRepositorySearcher<T, TId, TQuery, TRepo> where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new() where TRepo : IRecordRepository<T, TId, TQuery>
    {
        IAsyncEnumerable<TId> Search(TRepo repository, TQuery query, CancellationToken cancellationToken = default);
    }

    public interface IStatisticRepositorySearcher : IRecordRepositorySearcher<Statistic, string, StatisticQueryRequest, IStatisticRepository>
    {

    }

    public interface IPostRepositorySearcher : IRecordRepositorySearcher<Post, string, PostQueryRequest, IPostRepository>
    {

    }

    public interface IPageRepositorySearcher : IRecordRepositorySearcher<Page, string, PageQueryRequest, IPageRepository>
    {

    }

    public interface ILayoutRepositorySearcher : IRecordRepositorySearcher<Layout, string, LayoutQueryRequest, ILayoutRepository>
    {

    }

    public interface IFileRepositorySearcher : IRecordRepositorySearcher<File, string, FileQueryRequest, IFileRepository>
    {

    }

    public interface ICommentRepositorySearcher : IRecordRepositorySearcher<Comment, string, CommentQueryRequest, ICommentRepository>
    {

    }
}

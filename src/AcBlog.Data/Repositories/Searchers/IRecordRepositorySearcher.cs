using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.Searchers
{
    public interface IRecordRepositorySearcher<T, TId, TQuery, TRepo> where TId : class where T : class, IHasId<TId> where TQuery : QueryRequest, new() where TRepo : IRecordRepository<T, TId, TQuery>
    {
        TRepo Repository { get; }

        Task<QueryResponse<TId>> Search(TQuery query, CancellationToken cancellationToken = default);
    }

    public interface IPostRepositorySearcher: IRecordRepositorySearcher<Post,string,PostQueryRequest, IPostRepository>
    {

    }

    public interface ILayoutRepositorySearcher : IRecordRepositorySearcher<Layout, string, LayoutQueryRequest, ILayoutRepository>
    {

    }

    public interface ICommentRepositorySearcher : IRecordRepositorySearcher<Comment, string, CommentQueryRequest, ICommentRepository>
    {

    }

    public interface IStatisticRepositorySearcher : IRecordRepositorySearcher<Statistic, string, StatisticQueryRequest, IStatisticRepository>
    {

    }

    public interface IPageRepositorySearcher : IRecordRepositorySearcher<Page, string, PageQueryRequest, IPageRepository>
    {

    }
}

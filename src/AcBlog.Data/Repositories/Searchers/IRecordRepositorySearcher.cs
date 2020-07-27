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
}

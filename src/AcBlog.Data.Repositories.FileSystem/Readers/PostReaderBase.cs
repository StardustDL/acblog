using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public abstract class PostReaderBase : ReaderBase<Post, string>, IPostRepository
    {
        protected PostReaderBase(string rootPath) : base(rootPath)
        {
        }

        protected override string GetPath(string id) => Path.Join(RootPath, $"{id}.json").Replace("\\", "/");

        public override async Task<Post?> Get(string id)
        {
            var res = await base.Get(id);
            if (res != null)
                res.Id = id;
            return res;
        }

        public override async Task<IEnumerable<string>> All()
        {
            List<string> result = new List<string>();
            PostQueryRequest pq = new PostQueryRequest();
            while (true)
            {
                var req = await Query(pq);
                result.AddRange(req.Results);
                if (!req.CurrentPage.HasNextPage)
                    break;
                pq.Pagination = req.CurrentPage.NextPage();
            }
            return result;
        }

        public virtual async Task<QueryResponse<string>> Query(PostQueryRequest query)
        {
            query.Pagination ??= new Pagination();

            PagingPath? paging = null;

            if(query.Type != null)
            {
                switch (query.Type)
                {
                    case PostType.Article:
                        paging = new PagingPath(Path.Join(RootPath, "articles"));
                        break;
                    case PostType.Slides:
                        paging = new PagingPath(Path.Join(RootPath, "slides"));
                        break;
                    case PostType.Note:
                        paging = new PagingPath(Path.Join(RootPath, "notes"));
                        break;
                }
            }

            paging ??= new PagingPath(Path.Join(RootPath, "pages"));

            await EnsurePagingConfig(paging);
            paging.FillPagination(query.Pagination);

            var res = new QueryResponse<string>(await GetPagingResult(paging, query.Pagination), query.Pagination);
            return res;
        }
    }
}

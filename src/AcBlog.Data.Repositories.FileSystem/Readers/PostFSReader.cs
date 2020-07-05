using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem.Readers
{
    public class PostFSReader : RecordFSReaderBase<Post, string, PostQueryRequest>, IPostRepository
    {
        public PostFSReader(string rootPath, IFileProvider fileProvider) : base(rootPath, fileProvider)
        {
        }

        protected override string GetPath(string id) => Path.Join(RootPath, $"{NameUtility.Encode(id)}.json");

        public override Task<QueryResponse<string>> Query(PostQueryRequest query, CancellationToken cancellationToken = default)
        {
            query.Pagination ??= new Pagination();

            var paging = PagingProvider;

            if (query.Type != null)
            {
                switch (query.Type)
                {
                    case PostType.Article:
                        paging = new PagingProvider<string>(Path.Join(RootPath, "articles"), FileProvider);
                        break;
                    case PostType.Slides:
                        paging = new PagingProvider<string>(Path.Join(RootPath, "slides"), FileProvider);
                        break;
                    case PostType.Note:
                        paging = new PagingProvider<string>(Path.Join(RootPath, "notes"), FileProvider);
                        break;
                }
            }
            else if (query.Category != null && query.Category.Items.Any())
            {
                var catRoot = Path.Join(RootPath, "categories");
                catRoot = Path.Join(catRoot, Path.Combine(query.Category.Items.Select(NameUtility.Encode).ToArray()));
                paging = new PagingProvider<string>(catRoot, FileProvider);
            }
            else if (query.Keywords != null && query.Keywords.Items.Any())
            {
                var catRoot = Path.Join(RootPath, "keywords");
                paging = new PagingProvider<string>(Path.Join(catRoot, query.Keywords.Items.Select(NameUtility.Encode).First()), FileProvider);
            }

            paging.FillPagination(query.Pagination);

            var res = new QueryResponse<string>(
                paging.GetPaging(query.Pagination),
                query.Pagination);
            return Task.FromResult(res);
        }
    }
}

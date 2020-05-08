using AcBlog.Data.Models;
using AcBlog.Data.Repositories.FileSystem.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Linq;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories.FileSystem
{
    public static class PostRepositoryBuilder
    {
        public static async Task Build(IList<Post> data, string rootPath, int countPerPage)
        {
            if (countPerPage <= 0) countPerPage = 10;

            data = (from x in data orderby x.CreationTime descending select x).ToArray();

            string pagePath = Path.Join(rootPath, "pages");
            if (!Directory.Exists(pagePath))
                Directory.CreateDirectory(pagePath);

            PagingPath paging = new PagingPath(pagePath);

            {
                paging.Config = new PagingConfig
                {
                    CountPerPage = countPerPage,
                    TotalCount = data.Count
                };
                using var fs = File.Open(paging.ConfigPath, FileMode.Create, FileAccess.Write);
                await JsonSerializer.SerializeAsync(fs, paging.Config);
            }
            List<string> page = new List<string>();
            int pn = 0;
            foreach (var v in data)
            {
                {
                    using var fs = File.Open(Path.Join(rootPath, $"{v.Id}.json"), FileMode.Create, FileAccess.Write);
                    await JsonSerializer.SerializeAsync(fs, v);
                }
                page.Add(v.Id);
                if (page.Count == countPerPage)
                {
                    Pagination pg = new Pagination
                    {
                        PageNumber = pn
                    };
                    using var fs = File.Open(paging.GetPagePath(pg), FileMode.Create, FileAccess.Write);
                    await JsonSerializer.SerializeAsync(fs, page);
                    page.Clear();
                    pn++;
                }
            }
            if (page.Count > 0)
            {
                Pagination pg = new Pagination
                {
                    PageNumber = pn
                };
                using var fs = File.Open(paging.GetPagePath(pg), FileMode.Create, FileAccess.Write);
                await JsonSerializer.SerializeAsync(fs, page);
            }
        }
    }
}

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
using AcBlog.Data.Protections;

namespace AcBlog.Data.Repositories.FileSystem
{
    public static class PostRepositoryBuilder
    {
        static async Task BuildPaging(IList<string> data, string pagePath, int countPerPage)
        {
            if (countPerPage <= 0) countPerPage = 10;

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
                page.Add(v);
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
            if (page.Count > 0 || data.Count == 0)
            {
                Pagination pg = new Pagination
                {
                    PageNumber = pn
                };
                using var fs = File.Open(paging.GetPagePath(pg), FileMode.Create, FileAccess.Write);
                await JsonSerializer.SerializeAsync(fs, page);
            }
        }

        public static async Task Build(IList<(Post, ProtectionKey?)> data, IProtector<Post> protector, string rootPath, int countPerPage)
        {
            if (countPerPage <= 0) countPerPage = 10;

            data = (from x in data orderby x.Item1.CreationTime descending select x).ToArray();

            foreach (var v in data)
            {
                using var fs = File.Open(Path.Join(rootPath, $"{v.Item1.Id}.json"), FileMode.Create, FileAccess.Write);
                if (v.Item2 != null)
                    await JsonSerializer.SerializeAsync(fs, await protector.Protect(v.Item1, v.Item2));
                else
                    await JsonSerializer.SerializeAsync(fs, v.Item1);
            }

            await BuildPaging((from x in data select x.Item1.Id).ToList(),
                Path.Join(rootPath, "pages"), countPerPage);

            await BuildPaging((from x in data where x.Item1.Type == PostType.Article select x.Item1.Id).ToList(),
                Path.Join(rootPath, "articles"), countPerPage);

            await BuildPaging((from x in data where x.Item1.Type == PostType.Slides select x.Item1.Id).ToList(),
                Path.Join(rootPath, "slides"), countPerPage);

            await BuildPaging((from x in data where x.Item1.Type == PostType.Note select x.Item1.Id).ToList(),
                Path.Join(rootPath, "notes"), countPerPage);
        }
    }
}

using AcBlog.Data.Models;
using AcBlog.Data.Repositories.FileSystem.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AcBlog.Data.Repositories.FileSystem
{
    /* root/
     *   config.json
     *   pages/
     *     0.json
     *   id.json
    */

    public static class PostRepositoryBuilder
    {
        public static async Task Build(IList<Post> data, string rootPath, int countPerPage)
        {
            if (countPerPage <= 0) countPerPage = 10;

            string pagePath = Path.Join(rootPath, "pages");

            if (!Directory.Exists(pagePath))
                Directory.CreateDirectory(pagePath);
            {
                PostRepositoryConfig config = new PostRepositoryConfig
                {
                    CountPerPage = countPerPage,
                    TotalCount = data.Count
                };
                using var fs = File.Open(Path.Join(rootPath, "config.json"), FileMode.Create, FileAccess.Write);
                await JsonSerializer.SerializeAsync(fs, config);
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
                    using var fs = File.Open(Path.Join(pagePath, $"{pn}.json"), FileMode.Create, FileAccess.Write);
                    await JsonSerializer.SerializeAsync(fs, page);
                    page.Clear();
                    pn++;
                }
            }
            if (page.Count > 0)
            {
                using var fs = File.Open(Path.Join(pagePath, $"{pn}.json"), FileMode.Create, FileAccess.Write);
                await JsonSerializer.SerializeAsync(fs, page);
            }
        }
    }
}

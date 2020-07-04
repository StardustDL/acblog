using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories.FileSystem
{
    public static class BaseRepositoryBuilder
    {
        public static async Task SaveToFile<T>(string path, T value)
        {
            using var fs = File.Open(path, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(fs, value);
        }
    }

    public abstract class BaseRepositoryBuilder<T>
    {
        private int _countPerPage = 10;

        protected BaseRepositoryBuilder(IList<T> data, DirectoryInfo dist)
        {
            Data = data;
            Dist = dist;
        }

        public IList<T> Data { get; set; }

        public DirectoryInfo Dist { get; set; }

        public int CountPerPage
        {
            get => _countPerPage; set
            {
                if (value <= 0) value = 10;
                _countPerPage = value;
            }
        }

        protected abstract string GetId(T item);

        protected virtual IList<string> GetIds(IList<T> data) => data.Select(GetId).ToList();

        protected virtual async Task SaveItem(T item)
        {
            using var fs = File.Open(Path.Join(Dist.FullName, $"{GetId(item)}.json"), FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(fs, item);
        }

        public async Task BuildPaging(IList<T> data, DirectoryInfo root)
        {
            var ids = GetIds(data);

            root.Create();

            PagingPath paging = new PagingPath(root.FullName);
            {
                paging.Config = new PagingConfig
                {
                    PageSize = CountPerPage,
                    TotalCount = ids.Count
                };
                await BaseRepositoryBuilder.SaveToFile(paging.ConfigPath, paging.Config);
            }

            List<string> page = new List<string>();
            int pn = 0;
            foreach (var v in ids)
            {
                page.Add(v);
                if (page.Count == CountPerPage)
                {
                    Pagination pg = new Pagination
                    {
                        CurrentPage = pn
                    };
                    await BaseRepositoryBuilder.SaveToFile(paging.GetPagePath(pg)!, page);
                    page.Clear();
                    pn++;
                }
            }
            if (page.Count > 0 || ids.Count == 0)
            {
                Pagination pg = new Pagination
                {
                    CurrentPage = pn
                };
                await BaseRepositoryBuilder.SaveToFile(paging.GetPagePath(pg)!, page);
            }
        }

        protected Task BuildPages()
        {
            return BuildPaging(Data, Dist.CreateSubdirectory("pages"));
        }

        protected async Task BuildData()
        {
            foreach (var v in Data)
            {
                await SaveItem(v);
            }
        }

        public virtual async Task Build()
        {
            await BuildData();
            await BuildPages();
        }
    }
}

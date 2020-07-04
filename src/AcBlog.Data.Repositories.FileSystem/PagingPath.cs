using System.IO;
using AcBlog.Data.Models.Actions;

namespace AcBlog.Data.Repositories.FileSystem
{
    public class PagingPath
    {
        private PagingConfig? _config = null;

        public PagingPath(string rootPath)
        {
            RootPath = rootPath;
            ConfigPath = Path.Join(RootPath, "config.json").Replace("\\", "/");
        }

        public PagingConfig? Config
        {
            get => _config; set
            {
                if (value != null && value.CountPerPage <= 0)
                    value.CountPerPage = 10;
                _config = value;
            }
        }
        public string RootPath { get; }

        public string ConfigPath { get; }

        public string? GetPagePath(Pagination pagination)
        {
            if (Config is null)
                throw new System.Exception("No paging config loaded.");
            if (pagination.CurrentPage >= 0 && (pagination.CurrentPage < Config.TotalPage || Config.TotalPage == 0 && pagination.CurrentPage == 0))
            {
                return Path.Join(RootPath, $"{pagination.CurrentPage}.json").Replace("\\", "/");
            }
            else
            {
                return null;
            }
        }

        public void FillPagination(Pagination pagination)
        {
            if (Config is null)
                throw new System.Exception("No paging config loaded.");
            pagination.PageSize = Config.CountPerPage;
            pagination.TotalCount = Config.TotalCount;
        }
    }
}

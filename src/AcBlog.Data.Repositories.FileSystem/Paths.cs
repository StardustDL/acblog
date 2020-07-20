using AcBlog.Data.Models;
using System.IO;
using System.Linq;

namespace AcBlog.Data.Repositories.FileSystem
{
    public static class Paths
    {
        public static string GetFilterRoot(string rootPath) => Path.Join(rootPath, "filters");

        public static string GetPaginationRoot(string rootPath) => Path.Join(rootPath, "pages");

        public static string GetCategoryRoot(string rootPath) => Path.Join(GetFilterRoot(rootPath), "categories");

        public static string GetCategoryMetadata(string rootPath) => Path.Join(GetCategoryRoot(rootPath), "all.json");

        public static string GetKeywordRoot(string rootPath) => Path.Join(GetFilterRoot(rootPath), "keywords");

        public static string GetKeywordMetadata(string rootPath) => Path.Join(GetKeywordRoot(rootPath), "all.json");

        public static string GetArticleRoot(string rootPath) => Path.Join(GetFilterRoot(rootPath), "articles");

        public static string GetSlidesRoot(string rootPath) => Path.Join(GetFilterRoot(rootPath), "slides");

        public static string GetNoteRoot(string rootPath) => Path.Join(GetFilterRoot(rootPath), "notes");

        public static string GetCategoryRoot(string rootPath, Category category) => Path.Join(GetCategoryRoot(rootPath), Path.Combine(category.Items.Select(NameUtility.Encode).ToArray()));

        public static string GetKeywordRoot(string rootPath, Keyword keyword) => Path.Join(GetKeywordRoot(rootPath), Path.Combine(keyword.Items.Select(NameUtility.Encode).ToArray()));

        public static string GetFileById(string rootPath, string id) => Path.Join(rootPath, $"{NameUtility.Encode(id)}.json");
    }
}

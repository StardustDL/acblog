using AcBlog.Data.Models;
using AcBlog.Tools.SDK.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AcBlog.Tools.SDK.Helpers
{
    static class PathsExtensions
    {
        public static string GetPostRoot(this Workspace root)
        {
            return Path.Join(root.Root.FullName, "posts");
        }

        public static IEnumerable<string> GetPostFiles(this Workspace root)
        {
            return Directory.EnumerateFiles(root.GetPostRoot(), "*.md", SearchOption.AllDirectories);
        }

        public static string GetCategoryRoot(this Workspace root)
        {
            return Path.Join(root.Root.FullName, "categories");
        }

        public static IEnumerable<string> GetCategoryFiles(this Workspace root)
        {
            return Directory.EnumerateFiles(root.GetCategoryRoot(), "*.md", SearchOption.AllDirectories);
        }

        public static string GetKeywordRoot(this Workspace root)
        {
            return Path.Join(root.Root.FullName, "keywords");
        }

        public static IEnumerable<string> GetKeywordFiles(this Workspace root)
        {
            return Directory.EnumerateFiles(root.GetKeywordRoot(), "*.md", SearchOption.AllDirectories);
        }

        public static string GenerateItemPath(this Workspace root, Post post)
        {
            return Path.Join(root.GetPostRoot(), $"{post.Id}.md");
        }

        public static string GenerateItemPath(this Workspace root, Category category)
        {
            return Path.Join(root.GetCategoryRoot(), $"{category.Id}.md");
        }

        public static string GenerateItemPath(this Workspace root, Keyword keyword)
        {
            return Path.Join(root.GetKeywordRoot(), $"{keyword.Id}.md");
        }
    }
}

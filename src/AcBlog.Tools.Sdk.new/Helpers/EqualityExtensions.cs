using AcBlog.Data.Models;

namespace AcBlog.Tools.Sdk.Helpers
{
    internal static class EqualityExtensions
    {
        public static bool EqualsTo(this Post a, Post b)
        {
            try
            {
                Equality eq = new Equality();
                eq.Add(a.Id, b.Id);
                eq.Add(a.AuthorId, b.AuthorId);
                eq.Add(a.CategoryId, b.CategoryId);
                eq.Add(a.KeywordIds, b.KeywordIds);
                eq.Add(a.Type, b.Type);
                eq.Add(a.Title, b.Title);
                eq.Add(a.CreationTime, b.CreationTime);
                eq.Add(a.ModificationTime, b.ModificationTime);
                eq.Add(a.Content.Raw, b.Content.Raw);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool EqualsTo(this Category a, Category b)
        {
            try
            {
                Equality eq = new Equality();
                eq.Add(a.Id, b.Id);
                eq.Add(a.Name, b.Name);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool EqualsTo(this Keyword a, Keyword b)
        {
            try
            {
                Equality eq = new Equality();
                eq.Add(a.Id, b.Id);
                eq.Add(a.Name, b.Name);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}

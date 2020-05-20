using AcBlog.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.SDK
{
    public static class CategoriesServiceExtensions
    {
        public static Task<Category?[]> GetCategories(this ICategoryService service, IEnumerable<string> ids)
        {
            List<Task<Category?>> posts = new List<Task<Category?>>();
            foreach (var id in ids)
                posts.Add(service.Get(id));
            return Task.WhenAll(posts.ToArray());
        }
    }
}

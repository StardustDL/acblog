using AcBlog.Data.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.SDK
{
    public static class CategoryServiceExtensions
    {
        public static Task<Category?[]> GetCategories(this ICategoryService service, IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            List<Task<Category?>> posts = new List<Task<Category?>>();
            foreach (var id in ids)
                posts.Add(service.Get(id, cancellationToken));
            return Task.WhenAll(posts.ToArray());
        }
    }
}

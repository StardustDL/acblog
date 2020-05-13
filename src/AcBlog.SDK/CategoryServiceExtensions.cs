using AcBlog.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.SDK
{
    public static class CategoryServiceExtensions
    {
        public static async Task<IEnumerable<Category>> GetAllData(this ICategoryService service) => await service.GetData(await service.All());

        public static async Task<IEnumerable<Category>> GetData(this ICategoryService service, IEnumerable<string> ids)
        {
            List<Category> result = new List<Category>();
            foreach (var id in ids)
            {
                var item = await service.Get(id);
                if (item != null)
                    result.Add(item);
            }
            return result;
        }
    }
}

using AcBlog.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.SDK
{
    public static class KeywordsServiceExtensions
    {
        public static async Task<IEnumerable<Keyword>> GetAllData(this IKeywordService service) => await service.GetData(await service.All());

        public static async Task<IEnumerable<Keyword>> GetData(this IKeywordService service, IEnumerable<string> ids)
        {
            List<Keyword> result = new List<Keyword>();
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

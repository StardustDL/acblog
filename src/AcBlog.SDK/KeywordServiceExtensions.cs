using AcBlog.Data.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.SDK
{
    public static class KeywordServiceExtensions
    {
        public static Task<Keyword?[]> GetKeywords(this IKeywordService service, IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            List<Task<Keyword?>> posts = new List<Task<Keyword?>>();
            foreach (var id in ids)
                posts.Add(service.Get(id, cancellationToken));
            return Task.WhenAll(posts.ToArray());
        }
    }
}

using AcBlog.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Extensions
{
    public static class KeywordCollectionBuilder
    {
        public static async Task<(KeywordCollection, IReadOnlyDictionary<string, IList<Post>>)> BuildFromPosts(IAsyncEnumerable<Post> data, CancellationToken cancellationToken = default)
        {
            Dictionary<string, IList<Post>> dict = new Dictionary<string, IList<Post>>();
            await foreach (var v in data)
            {
                cancellationToken.ThrowIfCancellationRequested();
                List<string> keyids = new List<string>();
                foreach (var k in v.Keywords.Items)
                {
                    if (!dict.ContainsKey(k))
                    {
                        var list = new List<Post>
                        {
                            v
                        };
                        dict.Add(k, list);
                    }
                    else
                    {
                        dict[k].Add(v);
                    }
                }
            }
            return (new KeywordCollection(dict.Select(pair => new Keyword { Items = new string[] { pair.Key } }).ToList()), dict);
        }

        public static async Task<KeywordCollection> Build(IAsyncEnumerable<Keyword> data, CancellationToken cancellationToken = default)
        {
            HashSet<string> res = new HashSet<string>();
            await foreach (var v in data)
            {
                cancellationToken.ThrowIfCancellationRequested();
                List<string> keyids = new List<string>();
                foreach (var k in v.Items)
                {
                    res.Add(k);
                }
            }
            return new KeywordCollection(res.Select(pair => new Keyword { Items = new string[] { pair } }).ToList());
        }
    }
}

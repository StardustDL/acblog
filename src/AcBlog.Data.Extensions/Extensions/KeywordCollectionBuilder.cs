using AcBlog.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace AcBlog.Data.Extensions
{
    public static class KeywordCollectionBuilder
    {
        public static (KeywordCollection, IReadOnlyDictionary<string, IList<Post>>) BuildFromPosts(IEnumerable<Post> data)
        {
            Dictionary<string, IList<Post>> dict = new Dictionary<string, IList<Post>>();
            foreach (var v in data)
            {
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
            return (new KeywordCollection(dict.Select(pair => new Keyword(new string[] { pair.Key })).ToList()), dict);
        }
    }
}

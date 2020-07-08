using AcBlog.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace AcBlog.Data.Extensions
{
    public class KeywordCollector
    {
        public KeywordCollector(IList<Node> nodes) => Nodes = nodes;

        public IList<Node> Nodes { get; }

        public class Node
        {
            public Node(Keyword keyword, IList<Post> data)
            {
                Keyword = keyword;
                Data = data;
            }

            public Keyword Keyword { get; set; }

            public IList<Post> Data { get; }
        }

        public static KeywordCollector BuildFromPosts(IEnumerable<Post> data)
        {
            Dictionary<string, List<Post>> dict = new Dictionary<string, List<Post>>();
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
            return new KeywordCollector(
                dict.Select(
                    pair => new Node(
                        new Keyword(new string[] { pair.Key }),
                        pair.Value.ToArray())).ToArray());
        }
    }
}

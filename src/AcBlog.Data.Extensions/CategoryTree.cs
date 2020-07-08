using AcBlog.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcBlog.Data.Extensions
{
    public class CategoryTree
    {
        public CategoryTree(Node root) => Root = root;

        public Node Root { get; private set; }

        public class Node
        {
            public Node(Category category)
            {
                Category = category;
            }

            public Category Category { get; set; }

            public IList<Post> Data { get; } = new List<Post>();

            public Dictionary<string, Node> Children { get; } = new Dictionary<string, Node>();
        }

        public static CategoryTree BuildFromPosts(IEnumerable<Post> data)
        {
            Node root = new Node(new Category());
            foreach (var v in data)
            {
                Node node = root;
                foreach (var k in v.Category.Items)
                {
                    if (!node.Children.ContainsKey(k))
                    {
                        Category c = new Category
                        {
                            Items = node.Category.Items.Concat(new[] { k }),
                        };
                        var tn = new Node(c);
                        node.Children.Add(k, tn);
                        node = tn;
                    }
                    else
                    {
                        node = node.Children[k];
                    }
                    node.Data.Add(v);
                }
            }
            return new CategoryTree(root);
        }
    }
}

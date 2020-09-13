using AcBlog.Data.Models;
using AcBlog.Data.Models.Builders;
using System.Collections.Generic;
using System.Linq;

namespace AcBlog.Data.Extensions
{
    public static class CategoryTreeBuilder
    {
        public static (CategoryTree, IReadOnlyDictionary<CategoryTree.Node, IList<Post>>) BuildFromPosts(IEnumerable<Post> data)
        {
            CategoryTree.Node root = new CategoryTree.Node(new Category());
            Dictionary<CategoryTree.Node, IList<Post>> map = new Dictionary<CategoryTree.Node, IList<Post>>();
            foreach (var v in data)
            {
                CategoryTree.Node node = root;
                foreach (var k in v.Category.Items)
                {
                    if (!node.Children.ContainsKey(k))
                    {
                        CategoryBuilder cb = new CategoryBuilder();
                        cb.AddSubCategory(node.Category.Items.Concat(new[] { k }).ToArray());
                        Category c = cb.Build();
                        var tn = new CategoryTree.Node(c);
                        map.Add(tn, new List<Post>());
                        node.Children.Add(k, tn);
                        node = tn;
                    }
                    else
                    {
                        node = node.Children[k];
                    }
                    map[node].Add(v);
                }
            }
            return (new CategoryTree(root), map);
        }
    }
}

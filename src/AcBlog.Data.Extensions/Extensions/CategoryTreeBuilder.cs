using AcBlog.Data.Models;
using AcBlog.Data.Models.Builders;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Data.Extensions
{
    public static class CategoryTreeBuilder
    {
        public static async Task<(CategoryTree, IReadOnlyDictionary<CategoryTree.CategoryTreeNode, IList<Post>>)> BuildFromPosts(IAsyncEnumerable<Post> data, CancellationToken cancellationToken = default)
        {
            CategoryTree.CategoryTreeNode root = new CategoryTree.CategoryTreeNode(new Category());
            Dictionary<CategoryTree.CategoryTreeNode, IList<Post>> map = new Dictionary<CategoryTree.CategoryTreeNode, IList<Post>>();
            await foreach (var v in data)
            {
                cancellationToken.ThrowIfCancellationRequested();
                CategoryTree.CategoryTreeNode node = root;
                foreach (var k in v.Category.Items)
                {
                    if (!node.Children.ContainsKey(k))
                    {
                        CategoryBuilder cb = new CategoryBuilder();
                        cb.AddSubCategory(node.Category.Items.Concat(new[] { k }).ToArray());
                        Category c = cb.Build();
                        var tn = new CategoryTree.CategoryTreeNode(c);
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

        public static async Task<CategoryTree> Build(IAsyncEnumerable<Category> data, CancellationToken cancellationToken = default)
        {
            CategoryTree.CategoryTreeNode root = new CategoryTree.CategoryTreeNode(new Category());
            await foreach (var v in data)
            {
                cancellationToken.ThrowIfCancellationRequested();
                CategoryTree.CategoryTreeNode node = root;
                foreach (var k in v.Items)
                {
                    if (!node.Children.ContainsKey(k))
                    {
                        CategoryBuilder cb = new CategoryBuilder();
                        cb.AddSubCategory(node.Category.Items.Concat(new[] { k }).ToArray());
                        Category c = cb.Build();
                        var tn = new CategoryTree.CategoryTreeNode(c);
                        node.Children.Add(k, tn);
                        node = tn;
                    }
                    else
                    {
                        node = node.Children[k];
                    }
                }
            }
            return new CategoryTree(root);
        }
    }
}

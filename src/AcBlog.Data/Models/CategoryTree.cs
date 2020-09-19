using System.Collections.Generic;

namespace AcBlog.Data.Models
{
    public class CategoryTree
    {
        public CategoryTree() : this(new CategoryTreeNode()) { }

        public CategoryTree(CategoryTreeNode root) => Root = root;

        public CategoryTreeNode Root { get; set; }

        public class CategoryTreeNode
        {
            public CategoryTreeNode() : this(Category.Empty)
            {
            }

            public CategoryTreeNode(Category category)
            {
                Category = category;
            }

            public Category Category { get; set; }

            public Dictionary<string, CategoryTreeNode> Children { get; set; } = new Dictionary<string, CategoryTreeNode>();
        }

        public IEnumerable<Category> AsCategoryList()
        {
            Queue<CategoryTreeNode> q = new Queue<CategoryTreeNode>();
            foreach (var item in Root.Children.Values)
            {
                q.Enqueue(item);
            }
            while (q.Count > 0)
            {
                var u = q.Dequeue();
                yield return u.Category;
                foreach (var v in u.Children.Values)
                {
                    q.Enqueue(v);
                }
            }
        }
    }
}

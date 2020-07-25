using System.Collections.Generic;

namespace AcBlog.Data.Models
{
    public class CategoryTree
    {
        public CategoryTree() : this(new Node()) { }

        public CategoryTree(Node root) => Root = root;

        public Node Root { get; set; }

        public class Node
        {
            public Node() : this(Category.Empty)
            {
            }

            public Node(Category category)
            {
                Category = category;
            }

            public Category Category { get; set; }

            public Dictionary<string, Node> Children { get; set; } = new Dictionary<string, Node>();
        }

        public IEnumerable<Category> AsCategoryList()
        {
            Queue<Node> q = new Queue<Node>();
            foreach(var item in Root.Children.Values)
            {
                q.Enqueue(item);
            }
            while(q.Count > 0)
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

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
    }
}

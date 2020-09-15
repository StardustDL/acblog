namespace AcBlog.Data.Models.Actions
{
    public class PostQueryRequest : QueryRequest
    {
        public PostType? Type { get; set; }

        public string Author { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public Keyword? Keywords { get; set; }

        public Category? Category { get; set; }

        public PostQueryOrder Order { get; set; } = PostQueryOrder.None;
    }
}

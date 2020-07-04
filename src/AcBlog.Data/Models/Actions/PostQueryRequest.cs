using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Data.Models.Actions
{
    public class PostQueryRequest : QueryRequest
    {
        public PostType? Type { get; set; } = null;

        public string AuthorId { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Keywords { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public PostResponseOrder Order { get; set; } = PostResponseOrder.None;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Data.Models.Actions
{
    public class PostQueryRequest
    {
        public PostType? Type { get; set; } = null;

        public string Id { get; set; } = string.Empty;

        public string AuthorId { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public PostResponseOrder Order { get; set; } = PostResponseOrder.None;

        public Pagination? Paging { get; set; } = null;
    }
}

using System;

namespace AcBlog.Data.Models.Actions
{
    public class PostQueryResponse
    {
        public Post[] Results { get; set; } = Array.Empty<Post>();

        public Pagination? NextPage { get; set; } = null;
    }
}

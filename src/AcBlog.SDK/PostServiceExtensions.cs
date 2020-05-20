using AcBlog.Data.Models;
using AcBlog.SDK.Filters;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace AcBlog.SDK
{
    public static class PostServiceExtensions
    {
        public static PostArticleFilter CreateArticleFilter(this IPostService service)
        {
            return new PostArticleFilter(service);
        }

        public static PostSlidesFilter CreateSlidesFilter(this IPostService service)
        {
            return new PostSlidesFilter(service);
        }

        public static PostNoteFilter CreateNoteFilter(this IPostService service)
        {
            return new PostNoteFilter(service);
        }

        public static PostKeywordFilter CreateKeywordFilter(this IPostService service)
        {
            return new PostKeywordFilter(service);
        }

        public static PostCategoryFilter CreateCategoryFilter(this IPostService service)
        {
            return new PostCategoryFilter(service);
        }

        public static Task<Post?[]> GetPosts(this IPostService service,IEnumerable<string> ids)
        {
            List<Task<Post?>> posts = new List<Task<Post?>>();
            foreach (var id in ids)
                posts.Add(service.Get(id));
            return Task.WhenAll(posts.ToArray());
        }
    }
}

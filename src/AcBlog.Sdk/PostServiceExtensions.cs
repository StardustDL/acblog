using AcBlog.Data.Models;
using AcBlog.Sdk.Filters;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk
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

        public static Task<Post?[]> GetPosts(this IPostService service, IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            List<Task<Post?>> posts = new List<Task<Post?>>();
            foreach (var id in ids)
                posts.Add(service.Get(id, cancellationToken));
            return Task.WhenAll(posts.ToArray());
        }

        public static async Task<Post?[]> GetAllPosts(this IPostService service, CancellationToken cancellationToken = default)
        {
            return await service.GetPosts(await service.All());
        }
    }
}

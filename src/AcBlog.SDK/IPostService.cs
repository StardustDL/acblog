using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Protections;
using AcBlog.Data.Repositories;
using System.Threading.Tasks;

namespace AcBlog.SDK
{
    public interface IPostService : IPostRepository
    {
        IBlogService Blog { get; }

        IProtector<Post> Protector { get; }
    }

    public static class PostServiceExtensions
    {
        public static async Task<QueryResponse<Post>> QueryPost(this IPostService service, PostQueryRequest query)
        {
            var raw = await service.Query(query);
            return await raw.MapAsync(async (string id) => (await service.Get(id))!);
        }
    }
}

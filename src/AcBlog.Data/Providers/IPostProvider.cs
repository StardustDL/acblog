using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using System.Threading.Tasks;

namespace AcBlog.Data.Providers
{
    public interface IPostProvider : IRecordProvider<Post, string>
    {
        Task<PostQueryResponse> Query(PostQueryRequest query);
    }
}

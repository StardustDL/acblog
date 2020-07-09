using AcBlog.Data.Models;
using AcBlog.Sdk.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk
{
    public interface IBlogService
    {
        IPostService PostService { get; }

        Task<BlogOptions> GetOptions(CancellationToken cancellationToken = default);
    }
}

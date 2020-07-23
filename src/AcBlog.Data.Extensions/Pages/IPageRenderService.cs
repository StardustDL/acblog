using AcBlog.Data.Models;
using System.Threading.Tasks;

namespace AcBlog.Data.Pages
{
    public interface IPageRenderService
    {
        Task<string> Render(Page page, Layout? layout);
    }
}
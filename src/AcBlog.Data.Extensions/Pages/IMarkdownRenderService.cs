using System.Threading.Tasks;

namespace AcBlog.Data.Pages
{
    public interface IMarkdownRenderService
    {
        Task<string> RenderHtml(string markdown);
        Task<string> RenderPlainText(string markdown);
    }
}
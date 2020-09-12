using AcBlog.Data.Pages;
using Markdig;
using Microsoft.Extensions.DependencyInjection;
using StardustDL.RazorComponents.Markdown;
using System.Threading.Tasks;

namespace AcBlog.UI.Components
{
    public class MarkdownUIComponent : UIComponent
    {
        public MarkdownUIComponent()
        {
            AddStyleSheetResource("_content/StardustDL.RazorComponents.Markdown/prismjs/themes/prism.css");
            AddStyleSheetResource("_content/StardustDL.RazorComponents.Markdown/katex/katex.min.css");
            AddStyleSheetResource("_content/StardustDL.RazorComponents.Markdown/css/markdown.css");
            AddScriptResource("_content/StardustDL.RazorComponents.Markdown/component-min.js");
            AddScriptResource("_content/StardustDL.RazorComponents.Markdown/mermaid/mermaid.min.js");
            AddScriptResource("_content/StardustDL.RazorComponents.Markdown/prismjs/components/prism-core.min.js");
            AddScriptResource("_content/StardustDL.RazorComponents.Markdown/prismjs/plugins/autoloader/prism-autoloader.min.js");
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMarkdownComponentService, CustomMarkdownComponentService>();
            base.ConfigureServices(services);
        }

        class CustomMarkdownComponentService : MarkdownComponentService
        {
            public CustomMarkdownComponentService(IMarkdownRenderService service) => Service = service;

            IMarkdownRenderService Service { get; }

            public override Task<string> RenderHtml(string value)
            {
                return Service.RenderHtml(value);
            }
        }
    }
}

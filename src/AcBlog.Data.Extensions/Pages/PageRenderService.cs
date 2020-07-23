using AcBlog.Data.Models;
using Newtonsoft.Json;
using Scriban;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcBlog.Data.Pages
{
    public enum PageFeature
    {
        Markdown
    }

    public class PageRenderService : IPageRenderService
    {
        public PageRenderService(IMarkdownRenderService markdownRenderService) => MarkdownRenderService = markdownRenderService;

        public IMarkdownRenderService MarkdownRenderService { get; }

        public class Context
        {
            public string Title { get; set; } = string.Empty;

            public string Content { get; set; } = string.Empty;

            public Dictionary<string, dynamic> Properties { get; set; } = new Dictionary<string, dynamic>();
        }

        public async Task<string> Render(Page page, Layout? layout)
        {
            string content = page.Content;
            var features = new HashSet<string>(page.Features.Items.Select(x => x.ToLowerInvariant()));
            if (features.Contains(nameof(PageFeature.Markdown).ToLowerInvariant()))
            {
                content = await MarkdownRenderService.RenderHtml(content);
            }

            if (layout is null)
            {
                return content;
            }
            else
            {
                var template = Template.Parse(layout.Template);
                Context context = new Context
                {
                    Title = page.Title,
                    Content = content
                };
                foreach (var item in page.Properties.Raw)
                {
                    context.Properties.Add(item.Key, JsonConvert.DeserializeObject<dynamic>(item.Value));
                }
                return await template.RenderAsync(new
                {
                    Context = context,
                    Props = context.Properties,
                    Content = context.Content
                }, member => member.Name);
            }
        }
    }
}

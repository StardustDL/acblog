using AcBlog.Data.Extensions;
using AcBlog.Data.Models;
using AcBlog.Services;
using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace AcBlog.Services.Generators.Syndication
{
    public static class SyndicationExtensions
    {
        static MarkdownPipeline Pipeline { get; } = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

        public static async Task<SyndicationFeed> BuildSyndication(this IBlogService service, string baseAddress)
        {
            ClientUriGenerator generator = new ClientUriGenerator
            {
                BaseAddress = baseAddress
            };
            BlogOptions blogOptions = await service.GetOptions();
            SyndicationFeed feed = new SyndicationFeed(blogOptions.Name, blogOptions.Description, new Uri(baseAddress));
            SyndicationPerson author = new SyndicationPerson("", blogOptions.Onwer, baseAddress);
            feed.Authors.Add(author);
            Dictionary<string, SyndicationCategory> categoryMap = new Dictionary<string, SyndicationCategory>();
            {
                var cates = await service.PostService.GetCategories();
                foreach (var p in cates.AsCategoryList())
                {
                    var cate = new SyndicationCategory(p.ToString());
                    categoryMap.Add(p.ToString(), cate);
                    feed.Categories.Add(cate);
                }
            }
            {
                var posts = service.PostService.GetAllItems().IgnoreNull();
                List<SyndicationItem> items = new List<SyndicationItem>();
                await foreach (var p in posts)
                {
                    if (p is null)
                        continue;
                    var s = new SyndicationItem(p.Title,
                        SyndicationContent.CreateHtmlContent(Markdown.ToHtml(p.Content.Raw, Pipeline)),
                        new Uri(generator.Post(p)), p.Id, p.ModificationTime);
                    s.Authors.Add(author);

                    string summary;
                    if (await service.PostService.Protector.IsProtected(p.Content))
                    {
                        summary = "Protected Post";
                    }
                    else
                    {
                        summary = Markdown.ToPlainText(p.Content.Raw, Pipeline);
                    }
                    s.Summary = SyndicationContent.CreatePlaintextContent(summary.Length <= 100 ? summary : summary.Substring(0, 100));
                    s.Categories.Add(new SyndicationCategory(p.Category.ToString()));
                    if (categoryMap.TryGetValue(p.Category.ToString(), out var cate))
                        s.Categories.Add(cate);
                    s.PublishDate = p.CreationTime;
                    items.Add(s);
                }
                feed.Items = items.AsEnumerable();
            }

            return feed;
        }
    }
}

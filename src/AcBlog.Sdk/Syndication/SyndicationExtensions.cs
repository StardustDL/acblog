using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web;

namespace AcBlog.Sdk.Syndication
{
    public static class SyndicationExtensions
    {
        static MarkdownPipeline Pipeline { get; } = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

        public static async Task<SyndicationFeed> BuildSyndication(this IBlogService service, string baseAddress)
        {
            SyndicationFeed feed = new SyndicationFeed("Name", "Description", new Uri(baseAddress));
            SyndicationPerson author = new SyndicationPerson("", "Onwer", baseAddress);
            feed.Authors.Add(author);
            Dictionary<string, SyndicationCategory> categoryMap = new Dictionary<string, SyndicationCategory>();
            {
                /*var cates = await BlogService.CategoryService.GetCategories(await BlogService.CategoryService.All());
                foreach (var p in cates)
                {
                    var cate = new SyndicationCategory(p.Name);
                    categoryMap.Add(p.Id, cate);
                    feed.Categories.Add(cate);
                }*/
            }
            {
                var posts = await service.PostService.GetPosts(await service.PostService.All());
                List<SyndicationItem> items = new List<SyndicationItem>();
                foreach (var p in posts)
                {
                    var s = new SyndicationItem(p.Title,
                        SyndicationContent.CreateHtmlContent(Markdown.ToHtml(p.Content.Raw, Pipeline)),
                        new Uri($"{baseAddress}/posts/{HttpUtility.UrlEncode(p.Id)}"), p.Id, p.ModificationTime);
                    s.Authors.Add(author);

                    string summary = "";
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
                    /*if (categoryMap.TryGetValue(p.CategoryId, out var cate))
                        s.Categories.Add(cate);*/
                    s.PublishDate = p.CreationTime;
                    items.Add(s);
                }
                feed.Items = items.AsEnumerable();
            }

            return feed;
        }
    }
}

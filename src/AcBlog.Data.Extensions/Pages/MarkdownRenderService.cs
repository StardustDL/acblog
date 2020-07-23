using Markdig;
using Markdig.Extensions.MediaLinks;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AcBlog.Data.Pages
{
    public class MarkdownRenderService : IMarkdownRenderService
    {
        class BilibiliMediaLinkHost : IHostProvider
        {
            static string? MediaLink(Uri uri)
            {
                string path = uri.AbsolutePath;
                var items = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (items.Length >= 2 && items[0] == "video")
                {
                    return $"//player.bilibili.com/player.html?bvid={items[1]}";
                }
                else
                {
                    return null;
                }
            }

            static IHostProvider Inner { get; } = HostProviderBuilder.Create(
                    "www.bilibili.com", MediaLink, iframeClass: "bilibili");

            public bool TryHandle(Uri mediaUri, bool isSchemaRelative, out string iframeUrl) => Inner.TryHandle(mediaUri, isSchemaRelative, out iframeUrl);

            public string Class => Inner.Class;

            public bool AllowFullScreen => Inner.AllowFullScreen;
        }

        class NeteaseMusicMediaLinkHost : IHostProvider
        {
            static string? MediaLink(Uri uri)
            {
                try
                {
                    string url = uri.ToString();
                    url = url.Substring(uri.Scheme.Length + 3); // ://
                    string pre = "music.163.com/#/";
                    if (url.StartsWith(pre))
                    {
                        var items = url.Substring(pre.Length).Split('?');
                        var id = items[1].Split("&", StringSplitOptions.RemoveEmptyEntries).First(p => p.StartsWith("id="))?.Substring(3);
                        int type = 0;
                        if (items[0] == "song")
                            type = 2;
                        else if (items[0] == "playlist")
                            type = 0;
                        else if (items[0] == "album")
                            type = 1;
                        return $"//music.163.com/outchain/player?type={type}&id={id}&auto=0";
                    }
                }
                catch { }
                return null;
            }

            static IHostProvider Inner { get; } = HostProviderBuilder.Create(
                    "music.163.com", MediaLink, iframeClass: "neteasemusic");

            public bool TryHandle(Uri mediaUri, bool isSchemaRelative, out string iframeUrl) => Inner.TryHandle(mediaUri, isSchemaRelative, out iframeUrl);

            public string Class => Inner.Class;

            public bool AllowFullScreen => Inner.AllowFullScreen;
        }

        Lazy<MarkdownPipeline> Pipeline = new Lazy<MarkdownPipeline>(() =>
        {
            MediaOptions mediaOptions = new MediaOptions();
            mediaOptions.Hosts.Add(new BilibiliMediaLinkHost());
            mediaOptions.Hosts.Add(new NeteaseMusicMediaLinkHost());

            var builder = new MarkdownPipelineBuilder();
            builder.UseAbbreviations()
                .UseAutoIdentifiers()
                .UseCitations()
                .UseCustomContainers()
                .UseDefinitionLists()
                .UseEmphasisExtras()
                .UseFigures()
                .UseFooters()
                .UseFootnotes()
                .UseGridTables()
                .UseMediaLinks(mediaOptions)
                .UsePipeTables()
                .UseListExtras()
                .UseTaskLists()
                .UseAutoLinks()
                .UseSmartyPants()
                .UseEmojiAndSmiley();
            // .UseBootstrap()

            builder.UseMathematics();
            builder.UseDiagrams();

            builder.UseGenericAttributes(); // Must be last as it is one parser that is modifying other parsers
            var pipeline = builder.Build();
            return pipeline;
        });

        public Task<string> RenderPlainText(string markdown)
        {
            return Task.FromResult(Markdown.ToPlainText(markdown, Pipeline.Value));
        }

        public Task<string> RenderHtml(string markdown)
        {
            return Task.FromResult(Markdown.ToHtml(markdown, Pipeline.Value));
        }
    }
}

using Markdig;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Client.Components.Markdown
{
    static class CustomPipeline
    {
        public static MarkdownPipeline Build()
        {
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
                .UseMathematics()
                .UseMediaLinks()
                .UsePipeTables()
                .UseListExtras()
                .UseTaskLists()
                .UseDiagrams()
                .UseAutoLinks()
                .UseSmartyPants()
                .UseEmojiAndSmiley()
                .UseBootstrap()
                .UseGenericAttributes(); // Must be last as it is one parser that is modifying other parsers
            return builder.Build();
        }
    }
}

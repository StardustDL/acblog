using AcBlog.Data.Documents;
using Markdig;
using System;

namespace AcBlog.Data.Extensions
{
    public static class DocumentExtensions
    {
        public static int CountWords(this Document document)
        {
            return document.Raw.Length;
        }

        public static TimeSpan GetReadTime(this Document document)
        {
            const int WordPerMinute = 500;
            return TimeSpan.FromMinutes(document.CountWords() / (double)WordPerMinute);
        }

        public static string GetPreview(this Document document, int length = 300)
        {
            string pre = Markdown.ToPlainText(document.Raw);
            return pre.Length <= length ? pre : pre.Substring(0, length);
        }
    }
}

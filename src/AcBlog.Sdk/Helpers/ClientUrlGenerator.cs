using AcBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace AcBlog.Sdk.Helpers
{
    public class ClientUrlGenerator : IClientUrlGenerator
    {
        private string _baseAddress = string.Empty;

        public string BaseAddress { get => _baseAddress; set => _baseAddress = value.TrimEnd('/'); }

        public string Post(Post post) => $"{BaseAddress}/posts/{Uri.EscapeDataString(post.Id)}";

        public string Posts() => $"{BaseAddress}/posts";

        public string Articles() => $"{BaseAddress}/articles";

        public string Slides() => $"{BaseAddress}/slides";

        public string Notes() => $"{BaseAddress}/notes";

        public string Archives() => $"{BaseAddress}/archives";

        public string Categories() => $"{BaseAddress}/categories";

        public string Category(Category category) => $"{BaseAddress}/categories/{Uri.EscapeDataString(category.ToString())}";

        public string Keywords() => $"{BaseAddress}/keywords";

        public string Keyword(Keyword keyword) => $"{BaseAddress}/keywords/{Uri.EscapeDataString(keyword.ToString())}";

        public string Page(Page page) => $"{BaseAddress}/{page.Route}";
    }
}

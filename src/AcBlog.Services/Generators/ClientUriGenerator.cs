using AcBlog.Data.Models;
using System;

namespace AcBlog.Services.Generators
{
    public class ClientUriGenerator : IClientUriGenerator
    {
        private string _baseAddress = string.Empty;

        public string BaseAddress { get => _baseAddress; set => _baseAddress = value.TrimEnd('/'); }

        public string Post(Post post) => $"{BaseAddress}/posts/{Uri.EscapeDataString(post.Id)}";

        public string Search(string query = "") => $"{BaseAddress}/search/{Uri.EscapeDataString(query)}";

        public string Posts() => $"{BaseAddress}/posts";

        public string Articles() => $"{BaseAddress}/articles";

        public string Slides() => $"{BaseAddress}/slides";

        public string Notes() => $"{BaseAddress}/notes";

        public string Archives() => $"{BaseAddress}/archives";

        public string Categories() => $"{BaseAddress}/categories";

        public string Comments() => $"{BaseAddress}/comments";

        public string Category(Category category) => $"{BaseAddress}/categories/{Uri.EscapeDataString(category.ToString())}";

        public string Keywords() => $"{BaseAddress}/keywords";

        public string Keyword(Keyword keyword) => $"{BaseAddress}/keywords/{Uri.EscapeDataString(keyword.ToString())}";

        public string Page(Page page) => $"{BaseAddress}/{page.Route}";
    }
}

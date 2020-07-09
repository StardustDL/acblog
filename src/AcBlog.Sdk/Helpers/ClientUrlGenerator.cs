using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace AcBlog.Sdk.Helpers
{
    public class ClientUrlGenerator
    {
        private string _baseAddress = string.Empty;

        public string BaseAddress { get => _baseAddress; set => _baseAddress = value.TrimEnd('/'); }

        public string Post(string id) => $"{BaseAddress}/posts/{HttpUtility.UrlEncode(id)}";

        public string Posts() => $"{BaseAddress}/posts";

        public string Articles() => $"{BaseAddress}/articles";

        public string Slides() => $"{BaseAddress}/slides";

        public string Notes() => $"{BaseAddress}/notes";


    }
}

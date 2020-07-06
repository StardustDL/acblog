using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AcBlog.Client.WebAssembly.Models
{
    public class BlogSettings
    {
        public string Name { get; set; } = "AcBlog";

        public string Description { get; set; } = "A blog system based on WebAssembly.";

        public string IndexIconUrl { get; set; } = "icon.png";

        public string IndexBackgroundUrl { get; set; } = "";

        public string Footer { get; set; } = "";

        public int StartYear { get; set; } = DateTimeOffset.Now.Year;

        public string Onwer { get; set; } = "";
    }
}

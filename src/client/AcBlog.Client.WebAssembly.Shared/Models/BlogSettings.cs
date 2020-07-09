using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AcBlog.Client.WebAssembly.Models
{
    public class BlogSettings
    {

        public string IndexIconUrl { get; set; } = "icon.png";

        public string IndexBackgroundUrl { get; set; } = "";

        public string Footer { get; set; } = "";
    }
}

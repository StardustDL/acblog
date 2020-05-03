using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcBlog.Client.WASM.Models
{
    public class BlogSettings
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string Footer { get; set; }

        public int StartYear { get; set; }

        public string Onwer { get; set; }
    }
}

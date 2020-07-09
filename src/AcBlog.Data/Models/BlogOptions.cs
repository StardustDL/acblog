using System;

namespace AcBlog.Data.Models
{
    public class BlogOptions
    {
        public string Name { get; set; } = "AcBlog";

        public string Description { get; set; } = "A blog system based on WebAssembly.";

        public int StartYear { get; set; } = 2020;

        public string Onwer { get; set; } = "StardustDL";

        public PropertyCollection Properties { get; set; } = new PropertyCollection();
    }
}

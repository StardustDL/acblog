using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Data.Models
{
    public class Document
    {
        public string Raw { get; set; } = "";

        public int WordCount => Raw.Length;
    }
}

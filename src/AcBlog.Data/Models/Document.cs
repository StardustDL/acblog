using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Data.Models
{
    public class Document
    {
        public string Raw { get; set; } = "";

        public int WordCount => Raw.Length;

        public string GetPreview()
        {
            if (Raw.Length <= 300)
                return Raw;
            else
                return Raw.Substring(0, 300);
        }
    }
}

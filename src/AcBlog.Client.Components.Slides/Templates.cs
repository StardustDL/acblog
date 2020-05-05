using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Client.Components.Slides
{
    class Templates
    {
        const string Slides = @"<!DOCTYPE html>
<html>
<head>
<title>Title</title>
<meta charset=""utf-8"">
</head>
<body>
<textarea id=""source"">
@Value
</textarea>
<script src=""_content/AcBlog.Client.Components.Slides/remark/remark.min.js"">
</script>
<script>
var slideshow = remark.create();
</script>
</body>
</html>
";

        public static string RenderSlides(string value)
        {
            return Slides.Replace("@Value", value);
        }
    }
}

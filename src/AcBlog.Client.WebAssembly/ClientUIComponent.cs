using AcBlog.UI.Components;

namespace AcBlog.Client.WebAssembly
{
    public class ClientUIComponent : UIComponent
    {
        public ClientUIComponent()
        {
            AddStyleSheetResource("lib/bootstrap/css/bootstrap.min.css");
            AddStyleSheetResource("lib/mdi/css/materialdesignicons.min.css");
            AddStyleSheetResource("lib/markdown-css/github-markdown.css");
            AddStyleSheetResource("css/app.css");

            AddScriptResource("js/interop.js");
            AddScriptResource("_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js");
            AddScriptResource("lib/jquery/jquery.slim.min.js");
            AddScriptResource("lib/bootstrap/js/bootstrap.bundle.min.js");
        }
    }
}

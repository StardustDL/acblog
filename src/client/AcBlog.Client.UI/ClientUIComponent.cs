using AcBlog.UI.Components;
using AntDesign;

namespace AcBlog.Client.UI
{
    public class ClientUIComponent : UIComponent
    {
        public ClientUIComponent()
        {
            AddLocalStyleSheetResource("lib/bootstrap/css/bootstrap.min.css");
            AddStyleSheetResource("_content/StardustDL.RazorComponents.MaterialDesignIcons/mdi/css/materialdesignicons.min.css");
            AddLocalStyleSheetResource("lib/markdown-css/github-markdown.css");
            AddLocalStyleSheetResource("css/app.css");

            AddLocalScriptResource("js/interop.js");
            AddScriptResource("_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js");
            AddLocalScriptResource("lib/jquery/jquery.slim.min.js");
            // AddScriptResource("lib/bootstrap/js/bootstrap.bundle.min.js");
        }
    }
}

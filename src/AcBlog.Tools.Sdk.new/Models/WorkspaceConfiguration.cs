using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Tools.Sdk.Models
{
    public class WorkspaceConfiguration
    {
        public ServerConfiguration? Remote { get; set; } = null;

        public string Token { get; set; } = "";
    }
}

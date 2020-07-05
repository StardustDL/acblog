using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Tools.Sdk.Models
{
    public class WorkspaceOption
    {
        public string CurrentRemote { get; set; } = string.Empty;

        public Dictionary<string, RemoteOption> Remotes { get; set; } = new Dictionary<string, RemoteOption>();
    }
}

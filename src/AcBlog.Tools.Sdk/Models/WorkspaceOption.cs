using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Tools.Sdk.Models
{
    public class WorkspaceOption
    {
        public Dictionary<string, RemoteConfiguration> Remotes { get; set; } = new Dictionary<string, RemoteConfiguration>();
    }
}

using AcBlog.Data.Models;
using System.Collections.Generic;

namespace AcBlog.Tools.Sdk.Models
{
    public class WorkspaceOption
    {
        public string CurrentRemote { get; set; } = string.Empty;

        public Dictionary<string, RemoteOption> Remotes { get; set; } = new Dictionary<string, RemoteOption>();

        public PropertyCollection Properties { get; set; } = new PropertyCollection();
    }
}

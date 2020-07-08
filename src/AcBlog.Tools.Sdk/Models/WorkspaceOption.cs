using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Tools.Sdk.Models
{
    public class WorkspaceOption
    {
        public string CurrentRemote { get; set; } = string.Empty;

        public Dictionary<string, RemoteOption> Remotes { get; set; } = new Dictionary<string, RemoteOption>();

        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        public string GetProperty(string key, string defaultValue = "")
        {
            if(Properties.TryGetValue(key, out var res))
            {
                return res;
            }
            return defaultValue;
        }

        public void SetProperty(string key, string value)
        {
            Properties[key] = value;
        }
    }
}

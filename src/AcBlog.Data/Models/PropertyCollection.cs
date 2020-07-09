using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Data.Models
{
    public class PropertyCollection
    {
        public Dictionary<string, string> Raw { get; set; } = new Dictionary<string, string>();

        public string this[string key, string defaultValue = ""]
        {
            get
            {
                return Raw.TryGetValue(key, out var res) ? res : defaultValue;
            }
            set
            {
                Raw[key] = value;
            }
        }
    }
}

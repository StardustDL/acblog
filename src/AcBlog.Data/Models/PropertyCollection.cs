using System;
using System.Collections.Generic;
using System.Text;

namespace AcBlog.Data.Models
{
    public class PropertyCollection
    {
        public PropertyCollection() : this(new Dictionary<string, string>()) { }

        public PropertyCollection(Dictionary<string, string> raw) => Raw = raw;

        public Dictionary<string, string> Raw { get; set; }

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
